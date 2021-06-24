using UniModules.UniCore.Runtime.DataFlow;

namespace UniModules.UniGame.Core.Runtime.DataFlow.Extensions
{
    using System;
    using Interfaces;
    using UnityEngine;

    public static class LifeTimeComponentExtension 
    {
        public static ILifeTime GetAssetLifeTime(this GameObject gameObject)
        {
            var lifetimeComponent = gameObject.GetComponent<ILifeTime>() ?? 
                                    gameObject.AddComponent<LifeTimeComponent>();
            return lifetimeComponent;
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

        public static ILifeTime GetAssetLifeTime(this Component component) => component.gameObject.GetAssetLifeTime();
        
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
