﻿using UniCore.Runtime.ProfilerTools;
using UniModules.UniCore.Runtime.Extension;
using UnityEngine;

namespace UniModules.UniCore.Runtime.DataFlow
{
    using System;
    using System.Buffers;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UniGame.Core.Runtime.DataFlow;
    using global::UniGame.Core.Runtime;

    
    public class LifeTime : ILifeTime, IDisposable
    {
        #region static data

        private static readonly LifeTimeDefinition _editorLifeTime = new();
        
        public static readonly ILifeTime TerminatedLifetime;
        public static readonly int DefaultCapacity = 2;
        public static readonly ArrayPool<LifeTimeReference> ReferencePool;
        
        public static ILifeTime EditorLifeTime => _editorLifeTime;

        static LifeTime()
        {
            var completedLifetime = new LifeTime();
            completedLifetime.Release();
            
            TerminatedLifetime = completedLifetime;
            
            ReferencePool = ArrayPool<LifeTimeReference>.Create();
        }

        public static LifeTime Create()
        {
            return new LifeTime();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release(ref LifeTimeReference reference)
        {
            switch (reference.type)
            {
                case LifeTimeReferenceType.None:
                    break;
                case LifeTimeReferenceType.Reference:
                    break;
                case LifeTimeReferenceType.Disposable:
                    if(reference.reference is IDisposable disposable)
                        disposable.Dispose();
                    break;
                case LifeTimeReferenceType.Action:
                    if(reference.reference is Action action)
                        action.Invoke();
                    break;
                case LifeTimeReferenceType.TerminateLifeTime:
                    if(reference.reference is LifeTime relesedLifeTime)
                        relesedLifeTime.Release();
                    break;
                case LifeTimeReferenceType.RestartLifeTime:
                    if(reference.reference is LifeTime lifeTime)
                        lifeTime.Restart();
                    break;
            }
        }
        
        #region type convertion

        public static implicit operator CancellationToken(LifeTime lifeTime) => lifeTime.Token;

        #endregion

        #endregion
        
        private LifeTimeReference signleReference;
        private LifeTimeReference[] dependencies = Array.Empty<LifeTimeReference>();
        
        public int length = 0;
        private CancellationTokenSource _cancellationTokenSource;
        
        public readonly int id;
        
        public bool isTerminated;
 
        
        public LifeTime()
        {
            id = Unique.GetId();
        }
        
        /// <summary>
        /// is lifetime terminated
        /// </summary>
        public bool IsTerminated => isTerminated;

        public CancellationToken Token
        {
            get
            {
                if (isTerminated)
                    return new CancellationToken(true);
                _cancellationTokenSource ??= new CancellationTokenSource();
                return _cancellationTokenSource.Token;
            }
        }
        
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddCleanUpAction(Action cleanAction) 
        {
            if (cleanAction == null) return this;
            
            //call cleanup immediate. lite time already ended
            if (isTerminated) {
                cleanAction.Invoke();
                return this;
            }
            
            AddReference(cleanAction, LifeTimeReferenceType.Action);
            return this;
        }
        
        public void AddChildLifeTime(LifeTime lifeTime)
        {
            if (isTerminated) {
                lifeTime.Release();
                return;
            }
            
            AddReference(lifeTime, LifeTimeReferenceType.TerminateLifeTime);
        }
        
        public void AddChildRestartLifeTime(LifeTime lifeTime)
        {
            if (isTerminated) {
                lifeTime.Release();
                return;
            }
            
            AddReference(lifeTime, LifeTimeReferenceType.RestartLifeTime);
        }
    
        /// <summary>
        /// add child disposable object
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddDispose(IDisposable item)
        {
            if (isTerminated) {
                item.Cancel();
                return this;
            }
            
            AddReference(item, LifeTimeReferenceType.Disposable);
            return this;
        }

        /// <summary>
        /// save object from GC
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILifeTime AddRef(object o)
        {
            if (isTerminated) return this;
            
            AddReference(o, LifeTimeReferenceType.Reference);
            return this;
        }

        public void AddReference(object reference, LifeTimeReferenceType type)
        {
            var referenceData = new LifeTimeReference
            {
                type = type,
                reference = reference,
            };
            
            if (signleReference.type == LifeTimeReferenceType.None)
            {
                signleReference = referenceData;
                return;
            }

            if (dependencies.Length == 0 || length >= dependencies.Length)
            {
                var size = Mathf.Max(length, DefaultCapacity);
                var newSize = size << 1;
                var newArray = ReferencePool.Rent(newSize);

                if (dependencies.Length > 0)
                {
                    Array.Copy(dependencies, newArray, length);
                    ReferencePool.Return(dependencies);
                }
                
                dependencies = newArray;
            }
            
            dependencies[length] = referenceData;
            length++;
        }

        /// <summary>
        /// restart lifetime
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restart()
        {
            Release();
            isTerminated = false;
        }

        public void Dispose() => Release();
        
        /// <summary>
        /// invoke all cleanup actions
        /// </summary>
        public void Release()
        {
            if (isTerminated) return;
            
            isTerminated = true;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            if(dependencies == null) return;
            
            for (var i = length-1; i >= 0; i--)
            {
#if UNITY_EDITOR
                try
                {
                    Release(ref dependencies[i]);
                }
                catch (Exception e)
                {
                    GameLog.LogError(e);
                }
#else
                Release(ref dependencies[i]);
#endif
            }
            
            Release(ref signleReference);
            
            if(dependencies.Length > 0)
                ReferencePool.Return(dependencies);
            
            dependencies = Array.Empty<LifeTimeReference>();
            length = 0;
        }

        #if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnDomainReload()
        {
            Application.quitting -= OnApplicationQuitting;
            Application.quitting += OnApplicationQuitting;
            _editorLifeTime?.Release();
        }
        
        public static void OnApplicationQuitting()
        {
            Application.quitting -= OnApplicationQuitting;
            _editorLifeTime?.Release();
        }
        
        #endif

    }

    
    [Serializable]
    public struct LifeTimeReference
    {
        //[FieldOffset(0)] public LifeTime owner;
        public LifeTimeReferenceType type;
        public object reference;
    }
    
    public enum LifeTimeReferenceType : byte
    {
        None,
        Reference,
        Disposable,
        Action,
        TerminateLifeTime,
        RestartLifeTime
    }
}
