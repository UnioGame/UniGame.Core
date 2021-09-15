using System.Diagnostics;
using System.Threading;
using UniCore.Runtime.ProfilerTools;
using UniModules.UniGame.Core.Runtime.Utils;

namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System;
    using DataFlow.Interfaces;
    using Interfaces;
    using UniModules.UniCore.Runtime.DataFlow;
    using UnityEngine;

    public class LifetimeScriptableObject : ScriptableObject, 
        ILifeTime,
        ILifeTimeContext,
        IDisposable
    {
        private static Color _logColorEnable = new Color(0.30f, 0.8f, 0.490f);
        private static Color _logColorDisable = Color.magenta;

        private string _objectName;
        private Type _assetType;
        protected LifeTimeDefinition _lifeTimeDefinition;

        #region LifeTime API

        public ILifeTime AddCleanUpAction(Action cleanAction) => LifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => LifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => LifeTime.AddRef(o);

        public bool IsTerminated => LifeTime.IsTerminated;
        
        public CancellationToken TokenSource => LifeTime.TokenSource;

        public string Name => _objectName;

        public Type Type => _assetType;
        
        #endregion
                
        public ILifeTime LifeTime => _lifeTimeDefinition = _lifeTimeDefinition ?? new LifeTimeDefinition();

        public void Reset()
        {
            _lifeTimeDefinition?.Release();
            OnReset();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("@UnityEngine.Application.isPlaying")]
        [Sirenix.OdinInspector.Button]
#endif
        public void Dispose() => Reset();

        private void OnEnable()
        {
            _objectName = name;
            _assetType = GetType();
            _lifeTimeDefinition?.Release();
            _lifeTimeDefinition = _lifeTimeDefinition ?? new LifeTimeDefinition();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeChanged;
            LifetimeObjectData.Add(this);
            LogLifeTimeScriptableMessage(nameof(OnEnable),_logColorEnable);
            LifeTime.AddCleanUpAction(() => LogLifeTimeScriptableMessage("LifeTime Terminated",_logColorDisable));
#endif
            
            OnActivate();
        }

        [Conditional("UNITY_EDITOR")]
        private void LogLifeTimeScriptableMessage(string message,Color color)
        {
            if(Application.isPlaying)
                GameLog.Log($"LifetimeScriptableObject Name: {_objectName} Type: {_assetType.Name}  Message {message}",color);
        }
        
        private void OnDisable()
        {
            _lifeTimeDefinition?.Terminate();

            if (!UniApplication.IsPlaying)
                OnEditorDisabled();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
            LogLifeTimeScriptableMessage(nameof(OnDisable),_logColorDisable);
            LifetimeObjectData.Remove(this);
#endif
            OnDisabled();
        }
        
#if UNITY_EDITOR
        
        private void PlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            switch (state) {
                case UnityEditor.PlayModeStateChange.ExitingPlayMode:
                    Reset();
                    break;
            }
        }
        
#endif
        
        protected virtual void OnActivate() {}

        protected virtual void OnReset() {}

        protected virtual void OnDisabled() {}
        
        protected virtual void OnEditorDisabled() {}
        
        protected virtual void OnEditorActivate() {}

    }
}