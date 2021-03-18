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
        private static Color _logColor = new Color(0.30f, 0.8f, 0.490f);
        
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
        public void Dispose()
        {
            Reset();
        }

        private void OnEnable()
        {
            // конструкция скорее всего смысла не имеет поскольку дважды OnEnable на ScriptableObject не вызовется
            _lifeTimeDefinition?.Terminate();
            _lifeTimeDefinition = new LifeTimeDefinition();
            OnActivate();
        }

        private void OnDisable()
        {
            _lifeTimeDefinition?.Terminate();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
#endif
            
            OnDisabled();
        }

        private void Awake()
        {
            if(_lifeTimeDefinition == null)
                _lifeTimeDefinition = new LifeTimeDefinition();
            _lifeTimeDefinition?.Release();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeChanged;
#endif
            
        }

#if UNITY_EDITOR
        
        private void PlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            switch (state) {
                case UnityEditor.PlayModeStateChange.ExitingPlayMode:
                    _lifeTimeDefinition?.Release();
                    break;
            }
        }
        
#endif
        
        protected virtual void OnActivate() {}

        protected virtual void OnReset() {}

        protected virtual void OnDisabled()
        {
            
        }

    }
}