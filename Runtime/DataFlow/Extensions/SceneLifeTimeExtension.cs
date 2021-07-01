using System;
using System.Collections.Generic;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniModules.UniGame.Core.Runtime.DataFlow.Extensions
{
    public static class SceneLifeTimeExtension
    {
        private static Dictionary<int,Scene> _activeScenes = new Dictionary<int,Scene>();
        private static Dictionary<int,ILifeTime> _sceneLifeTimes = new Dictionary<int,ILifeTime>(8);

        static SceneLifeTimeExtension()
        {
            SceneManager.sceneLoaded += (scene, mode) => _activeScenes[scene.handle] = scene;
            SceneManager.sceneUnloaded += scene => _activeScenes.Remove(scene.handle);

#if UNITY_EDITOR
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                _activeScenes.Clear();
                _sceneLifeTimes.Clear();
            };
#endif

        }

        public static ILifeTime GetSceneLifeTime(this Scene scene)
        {
            return _sceneLifeTimes.TryGetValue(scene.handle, out var lifeTime) 
                ? lifeTime : LifeTime.TerminatedLifetime;
        }
        
        public static ILifeTime GetLifeTime(this Scene scene)
        {
            return _sceneLifeTimes.TryGetValue(scene.handle, out var lifeTime) 
                ? lifeTime : LifeTime.TerminatedLifetime;
        }

        public static ILifeTime GetLifeTime(this object asset)
        {
            switch (asset)
            {
                case Scene scene:
                    return scene.GetLifeTime();
                case GameObject gameObject:
                    return gameObject.GetAssetLifeTime();
                case Component component:
                    return component.gameObject.GetAssetLifeTime();
                case ILifeTime lifeTime:
                    return lifeTime;
                case ILifeTimeContext lifeTimeContext:
                    return lifeTimeContext.LifeTime;
            }
            return LifeTime.TerminatedLifetime;
        }

        public static ILifeTime AddTo(this IDisposable disposable, Scene scene)
        {
            return AddTo(disposable, scene.handle);
        }

        public static ILifeTime AddTo(this IDisposable disposable,int sceneId)
        {
            if (!_activeScenes.ContainsKey(sceneId))
            {
                disposable.Dispose();
                return LifeTime.TerminatedLifetime;
            }

            if (!_sceneLifeTimes.TryGetValue(sceneId, out var sceneLifeTime))
            {
                var scene = _activeScenes[sceneId];
                sceneLifeTime = CreateSceneLifeTime(scene);
                sceneLifeTime.AddCleanUpAction(() => _sceneLifeTimes.Remove(sceneId));
            }

            return sceneLifeTime.AddDispose(disposable);
        }

        public static ILifeTime CreateSceneLifeTime(Scene scene)
        {
            var sceneObject = new GameObject();
            SceneManager.MoveGameObjectToScene(sceneObject,scene);
            var lifeTime = sceneObject.AddComponent<LifeTimeComponent>();
            return lifeTime;
        }
    }
}
