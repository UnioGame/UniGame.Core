namespace UniModules.UniGame.Core.Runtime.DataFlow.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using global::UniGame.Core.Runtime;
    using UniModules.UniCore.Runtime.DataFlow;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UniCore.Runtime.Utils;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;
#endif
    
    public static class SceneLifeTime
    {
        private static MemorizeItem<string,ILifeTime> _sceneLifeTimes = MemorizeTool
            .Memorize<string,ILifeTime>(sceneId =>
            {
                var sceneCount = SceneManager.sceneCount;
                Scene scene = default;

                for (var i = 0; i < sceneCount; i++)
                {
                    var targetScene = SceneManager.GetSceneAt(i);
                    if(!sceneId.Equals(targetScene.path,StringComparison.OrdinalIgnoreCase))
                        continue;
                    scene = targetScene;
                }
                
                if(string.IsNullOrEmpty(scene.path)) 
                    return LifeTime.TerminatedLifetime;
                
                var sceneObject = new GameObject();
                SceneManager.MoveGameObjectToScene(sceneObject,scene);
                var sceneLifeTime = sceneObject.GetAssetLifeTime();
                return sceneLifeTime;
            });

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnSceneModeChanged;
                
            static void OnSceneModeChanged(PlayModeStateChange state)
            {
                _sceneLifeTimes.Dispose();
            }
#endif
            static void OnSceneUnloaded(Scene scene)
            {
                _sceneLifeTimes.Remove(scene.path);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime GetActiveSceneLifeTime()
        {
            var activeScene = SceneManager.GetActiveScene();
            return GetSceneLifeTime(activeScene);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime GetSceneLifeTime(this Scene scene)
        {
            return _sceneLifeTimes[scene.path];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime GetLifeTime(this Scene scene)
        {
            return GetSceneLifeTime(scene);
        }

        public static ILifeTime GetLifeTime(this object asset)
        {
            if (asset is GameObject gameObjectAsset)
                return gameObjectAsset.GetAssetLifeTime();
            
            if (asset is Component componentAsset)
                return componentAsset.GetAssetLifeTime();
            
            switch (asset)
            {
                case Scene scene:
                    return scene.GetSceneLifeTime();
                case ILifeTime lifeTime:
                    return lifeTime;
                case ILifeTimeContext lifeTimeContext:
                    return lifeTimeContext.LifeTime;
            }
            
            return LifeTime.TerminatedLifetime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime AddTo(this IDisposable disposable, Scene scene)
        {
            return AddTo(disposable, scene.path);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime AddToActiveScene(this IDisposable disposable)
        {
            var activeScene = SceneManager.GetActiveScene();
            return AddTo(disposable, activeScene.path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILifeTime AddTo(this IDisposable disposable,string sceneId)
        {
            var sceneLifeTime = _sceneLifeTimes[sceneId];
            return sceneLifeTime.AddDispose(disposable);
        }

    }
}
