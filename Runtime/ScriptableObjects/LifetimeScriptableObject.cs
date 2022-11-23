namespace UniGame.Core.Runtime.ScriptableObjects
{
    using Runtime;
    using UniModules.UniGame.Core.Runtime.Utils;
    using System;
    using UniModules.UniCore.Runtime.DataFlow;
    using UnityEngine;

    public class LifetimeScriptableObject : ScriptableObject, 
        ILifeTimeContext,
        IDisposable
    {
        private string _objectName;
        private Type _assetType;
        private LifeTimeDefinition _lifeTimeDefinition;

        public string Name => _objectName;

        public Type Type => _assetType;
                
        public ILifeTime LifeTime => _lifeTimeDefinition ??= new LifeTimeDefinition();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("@UnityEngine.Application.isPlaying")]
        [Sirenix.OdinInspector.Button]
#endif
        public void Dispose() => _lifeTimeDefinition?.Release();

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            _objectName = name;
            _assetType = GetType();
            _lifeTimeDefinition?.Release();
            _lifeTimeDefinition ??= new LifeTimeDefinition();
            
            OnActivate();
        }

        private void OnDisable()
        {
            _lifeTimeDefinition?.Terminate();

            if (!UniApplication.IsPlaying)
                OnEditorDisabled();
        
            OnDisabled();
        }
                
        protected virtual void OnActivate() {}

        protected virtual void OnDisabled() {}
        
        protected virtual void OnEditorDisabled() {}
        
        protected virtual void OnEditorActivate() {}

    }
    
    
}