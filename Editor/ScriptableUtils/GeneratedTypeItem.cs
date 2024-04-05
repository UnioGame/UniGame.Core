using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    
#if UNITY_EDITOR
    
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.ReflectionUtils;
    using UniCore.Runtime.Utils;
    using UniModules.Editor;
    using UniModules.UniGame.Core.EditorTools.Editor;
    using UnityEditor;
    using UnityEngine;

    public static class GeneratedTypeItem
    {
        private static MemorizeItem<Type, ScriptableObject> _assetCache = 
            MemorizeTool.Memorize<Type, ScriptableObject>(null);
        
        private static MemorizeItem<Type, string> _pathCache = 
            MemorizeTool.Memorize<Type, string>(null);
        
        private static bool _initialized;
        private static string _assetPath;
        private static Dictionary<Type,List<Action<ScriptableObject>>> _callbacks = new();
        
        public static bool IsInitialized => _initialized;

        public static string GetAssetPath<TAsset>(bool includeNamespace = false) where TAsset : ScriptableObject
        {
            var targetType = typeof(TAsset);
            return GetAssetPath(targetType,includeNamespace);
        }
        
        public static string GetAssetPath(Type targetType,bool includeNamespace = false)
        {
            if(_pathCache.ContainsKey(targetType))
                return _pathCache[targetType];
            
            var generatedPath = EditorPathConstants.GeneratedContentDefaultPath;
            var assetPath = includeNamespace 
                ? generatedPath.CombinePath(targetType.Namespace).CombinePath(targetType.Name)
                : generatedPath.CombinePath(targetType.Name);
            
            _pathCache[targetType] = assetPath;
            
            return assetPath;
        }

        public static TAsset LoadAsset<TAsset>() where TAsset : ScriptableObject
        {
            return LoadAsset<TAsset>(static x => Preload(x));
        }

        public static TAsset LoadAsset<TAsset>(Action<TAsset> callback) where TAsset : ScriptableObject
        {
            var assetType = typeof(TAsset);
            
            return LoadAsset(assetType, x => callback?.Invoke(x as TAsset)) as TAsset;
        }
        
        public static ScriptableObject LoadAsset(Type assetType,Action<ScriptableObject> callback)
        {
            if (_initialized)
            {
                if (_assetCache.ContainsKey(assetType))
                {
                    callback?.Invoke(_assetCache[assetType]);
                }

                var asset = LoadAssetInternal( assetType);
                return asset;
            }

            if (!_callbacks.TryGetValue(assetType, out var callbacks))
            {
                callbacks = new List<Action<ScriptableObject>>();
                _callbacks[assetType] = callbacks;
            }
            
            callbacks.Add(callback);
            return null;
        }

        private static void Preload(ScriptableObject asset)
        {
            
        }
        
        private static ScriptableObject LoadAssetInternal(Type targetType)
        {
            var info = targetType.GetCustomAttribute<GeneratedAssetInfoAttribute>();

            var path = info == null || string.IsNullOrEmpty(info.Location) 
                ? GetAssetPath(targetType)
                : EditorPathConstants.GeneratedContentDefaultPath.CombinePath(info.Location);
                
            GameLog.Log($"GeneratedAsset Create asset of type {targetType.Name} : with path : {path}");

            _pathCache[targetType] = path;
            
            var newAsset  = AssetEditorTools.LoadOrCreate<ScriptableObject>(targetType, path,targetType.Name);

            if (!_callbacks.TryGetValue(targetType, out var callbacks)) 
                return newAsset;
            
            foreach (var callback in callbacks)
                callback?.Invoke(newAsset);
            
            callbacks.Clear();

            return newAsset;
        }

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorApplication.delayCall -= OnDelayedCall;
            EditorApplication.delayCall += OnDelayedCall;
        }
        
        private static void OnDelayedCall()
        {
            _initialized = true;

            foreach (var callbackValue in _callbacks)
                LoadAssetInternal(callbackValue.Key);
        }
        
    }

#endif
    
}
