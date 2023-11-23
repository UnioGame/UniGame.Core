using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    
#if UNITY_EDITOR
    
    using System;
    using System.Collections.Generic;
    using UniModules.Editor;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UniModules.UniGame.Core.EditorTools.Editor;
    using UnityEditor;
    using UnityEngine;
    
    public static class GeneratedTypeItem<TAsset>
        where TAsset : ScriptableObject
    {
        public readonly static Type ProcessorType = typeof(TAsset);

        private static bool _initialized;
        private static bool _assetRequested;
        private static string _assetPath;
        private static TAsset _selector;
        private static List<Action<TAsset>> _callbacks = new();

        public static string AssetPath
        {
            get
            {
                _assetPath ??= EditorPathConstants.GeneratedContentDefaultPath
                    .CombinePath($"GeneratedAsset/Editor/{ProcessorType.Namespace}/{ProcessorType.Name}");
                return _assetPath;
            }
            private set => _assetPath = value;
        }
        
        public static bool IsInitialized => _initialized;
        
        public static TAsset Asset => Load();

        public static void Load(Action<TAsset> callback)
        {
            if (_initialized && _selector != null)
            {
                callback?.Invoke(_selector);
                return;
            }
            
            _callbacks.Add(callback);
        }
        
        public static TAsset Load()
        {
            if (!_initialized)
            {
                _assetRequested = true;
                return default;
            }
            
            if (_selector != null) return _selector;
            
            GameLog.Log($"GeneratedAsset Create asset of type {nameof(TAsset)} : with path : {AssetPath}");
   
            var info = ProcessorType.GetCustomAttribute<GeneratedAssetInfoAttribute>();
            var path = info == null || string.IsNullOrEmpty(info.Location) 
                ? AssetPath 
                : EditorPathConstants.GeneratedContentDefaultPath.CombinePath(info.Location);
                
            AssetPath = path;
                
            var newAsset  = AssetEditorTools.LoadOrCreate<TAsset>(ProcessorType, path);
            _selector = newAsset;

            foreach (var callback in _callbacks)
                callback?.Invoke(_selector);
            
            _callbacks.Clear();
            return _selector;
        }
        
        [InitializeOnLoadMethod]
        public static void EditorInitialize()
        {
            EditorApplication.delayCall -= OnDelayedCall;
            EditorApplication.delayCall += OnDelayedCall;
        }

        private static void OnDelayedCall()
        {
            _initialized = true;
            if (_assetRequested) Load();
        }
        
    }

#endif
    
}
