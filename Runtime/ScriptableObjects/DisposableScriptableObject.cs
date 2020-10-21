using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using Interfaces;
    using UnityEngine;

    public class DisposableScriptableObject : 
        LifetimeScriptableObject,
        IResourceDisposable
    {
        public void Dispose()
        {
            if (_lifeTimeDefinition.IsTerminated)
                return;
            
            GameLog.Log($"DisposableAsset: {GetType().Name} {name} : DISPOSED",Color.blue,this);
            
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }

    }
}