namespace UniModules.UniCore.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Interfaces;
    using ObjectPool.Runtime.Interfaces;
    using Rx.Extensions;
    using UniGame.Core.Runtime.DataFlow;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public class LifeTime : ILifeTime, IPoolable
    {
        public readonly static ILifeTime TerminatedLifetime;
        
        private List<IDisposable> disposables = new List<IDisposable>();
        private List<object> referencies = new List<object>();
        private List<Action> cleanupActions = new List<Action>();
        private CancellationTokenSource _cancellationTokenSource;
        
        public readonly int id;
        
        public bool isTerminated;
        
#region static data
        
        static LifeTime()
        {
            var completedLifetime = new LifeTime();
            completedLifetime.Release();
            TerminatedLifetime = completedLifetime;
        }

        public static LifeTime Create()
        {
            return new LifeTime();
        }
        
#endregion
        
        private LifeTime()
        {
            id = Unique.GetId();
        }

        
        /// <summary>
        /// is lifetime terminated
        /// </summary>
        public bool IsTerminated => isTerminated;

        public CancellationTokenSource CancellationTokenSource =>
            _cancellationTokenSource ??= new CancellationTokenSource();
        
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        public ILifeTime AddCleanUpAction(Action cleanAction) 
        {
            if (cleanAction == null)
                return this;
            //call cleanup immediate. lite time already ended
            if (isTerminated) {
                cleanAction?.Invoke();
                return this;
            }
            cleanupActions.Add(cleanAction);
            return this;
        }
    
        /// <summary>
        /// add child disposable object
        /// </summary>
        public ILifeTime AddDispose(IDisposable item)
        {
            if (isTerminated) {
                item.Cancel();
                return this;
            }
            
            disposables.Add(item);
            return this;
        }

        /// <summary>
        /// save object from GC
        /// </summary>
        public ILifeTime AddRef(object o)
        {
            if (isTerminated)
                return this;
            referencies.Add(o);
            return this;
        }

        /// <summary>
        /// restart lifetime
        /// </summary>
        public void Restart()
        {
            Release();
            isTerminated = false;
        }
        
        /// <summary>
        /// invoke all cleanup actions
        /// </summary>
        public void Release()
        {
            if (isTerminated)
                return;
            
            isTerminated = true;
            
            for (var i = cleanupActions.Count-1; i >= 0; i--)
            {
                cleanupActions[i]?.Invoke();
            }
            
            for (var i = disposables.Count-1; i >= 0; i--) {
                disposables[i].Cancel();
            }

            cleanupActions.Clear();
            disposables.Clear();
            referencies.Clear();

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        #region type convertion

        public static implicit operator CancellationTokenSource(LifeTime lifeTime)
        {
            return lifeTime.CancellationTokenSource;
        } 

        #endregion
    }
}
