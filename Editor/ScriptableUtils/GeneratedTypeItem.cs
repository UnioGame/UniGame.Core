using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    
#if UNITY_EDITOR
    
    using System;
    using UniModules.Editor;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UniModules.UniGame.Core.EditorTools.Editor;
    using UnityEngine;
    
    public static class GeneratedTypeItem<TAsset>
        where TAsset : ScriptableObject
    {
        public readonly static Type ProcessorType = typeof(TAsset);

        private static string assetPath;
        private static TAsset selector;

        public static string AssetPath
        {
            get
            {
                assetPath ??= EditorPathConstants.GeneratedContentDefaultPath.CombinePath($"GeneratedAsset/Editor/{ProcessorType.Namespace}/{ProcessorType.Name}");
                return assetPath;
            }
            private set
            {
                assetPath = value;
            }
        }
        
        public static TAsset Asset
        {
            get
            {
                if (selector) return selector;
                selector = Create();
                return selector;
            }
        }
        
        
        public static TAsset Create()
        {
            GameLog.Log($"GeneratedAsset Create asset of type {nameof(TAsset)} : with path : {AssetPath}");
   
            var info = ProcessorType.GetCustomAttribute<GeneratedAssetInfoAttribute>();
            var path = info == null || string.IsNullOrEmpty(info.Location) ? AssetPath : 
                EditorPathConstants.GeneratedContentDefaultPath.CombinePath(info.Location);
                
            AssetPath = path;
                
            var newAsset  = AssetEditorTools.LoadOrCreate<TAsset>(ProcessorType, path);
            return newAsset;
        }
        
    }

#endif
    
}
