using System.Diagnostics;
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
        
        protected LifeTimeDefinition _lifeTimeDefinition;

        #region LifeTime API

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTimeDefinition.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTimeDefinition.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTimeDefinition.AddRef(o);

        public bool IsTerminated => _lifeTimeDefinition.IsTerminated;
        
        #endregion
                
        public ILifeTime LifeTime => _lifeTimeDefinition;

        public void Reset()
        {
            _lifeTimeDefinition?.Release();
            OnReset();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Dispose() => Reset();

        private void OnEnable()
        {
            _lifeTimeDefinition?.Release();
            _lifeTimeDefinition = new LifeTimeDefinition();
            
            if (!UniApplication.IsPlaying)
            {
                OnEditorActivate();
                return;
            }
            
#if UNITY_EDITOR
            LifetimeObjectData.Add(this);
            LogLifeTimeScriptableMessage(nameof(OnEnable),_logColorEnable);
            _lifeTimeDefinition.AddCleanUpAction(() => LogLifeTimeScriptableMessage("LifeTime Terminated",_logColorDisable));
#endif
            
            OnActivate();
        }

        [Conditional("UNITY_EDITOR")]
        private void LogLifeTimeScriptableMessage(string message,Color color)
        {
            GameLog.Log($"LifetimeScriptableObject Name: {name} Type: {GetType().Name}  Message {message}",color);
        }
        
        private void OnDisable()
        {
            _lifeTimeDefinition?.Terminate();

            if (!UniApplication.IsPlaying)
            {
                OnEditorDisabled();
            }
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
            LogLifeTimeScriptableMessage(nameof(OnDisable),_logColorDisable);
            LifetimeObjectData.Remove(this);
#endif
            OnDisabled();
        }

        private void Awake()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeChanged;
#endif
            
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