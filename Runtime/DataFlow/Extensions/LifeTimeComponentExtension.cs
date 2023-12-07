using UniModules.UniCore.Runtime.DataFlow;

namespace UniModules.UniGame.Core.Runtime.DataFlow.Extensions
{
    using System;
    using global::UniGame.Core.Runtime;
    using UnityEngine;

    public static class LifeTimeComponentExtension 
    {
        public static ILifeTime GetAssetLifeTime(this GameObject gameObject, bool terminateOnDisable = false)
        {
            var lifetimeComponent = gameObject.GetComponent<LifeTimeComponent>(); 

            lifetimeComponent = lifetimeComponent != null
                ? lifetimeComponent
                : gameObject.AddComponent<LifeTimeComponent>();
            
            return terminateOnDisable 
                ? lifetimeComponent.DisableLifeTime 
                : lifetimeComponent;
        }

        public static ILifeTime DestroyOnCleanup(this LifeTime lifeTime, GameObject gameObject)
        {
            lifeTime.AddCleanUpAction(() =>
            {
                if (gameObject)
                    UnityEngine.Object.Destroy(gameObject);
            });
            return lifeTime;
        }
        
        public static ILifeTime DestroyOnCleanup(this LifeTime lifeTime, Component component, bool onlyComponent = false)
        {
            if (!onlyComponent)
            {
                return lifeTime.DestroyOnCleanup(component.gameObject);
            }
            
            lifeTime.AddCleanUpAction(() =>
            {
                if (component)
                    UnityEngine.Object.Destroy(component);
            });
            
            return lifeTime;
        }

        public static ILifeTime GetAssetLifeTime(this Component component, bool terminateOnDisable = false)
        {
            return component.gameObject.GetAssetLifeTime(terminateOnDisable);
        }
        
        public static ILifeTime AddTo(this GameObject gameObject, IDisposable disposable)
        {
            return gameObject.GetAssetLifeTime().AddDispose(disposable);
        }
        
        public static ILifeTime AddCleanUp(this GameObject gameObject, Action cleanupAction)
        {
            return gameObject.GetAssetLifeTime().AddCleanUpAction(cleanupAction);
        }
        
        public static ILifeTime AddTo(this Component component, IDisposable disposable) => AddTo(component.gameObject, disposable);
        
        public static ILifeTime AddTo(this Component component, Action action) =>AddCleanUp(component.gameObject, action);
        
        
        public static ILifeTime AddCleanUp(this Component component, Action cleanupAction) => AddCleanUp(component.gameObject, cleanupAction);

    }
}
