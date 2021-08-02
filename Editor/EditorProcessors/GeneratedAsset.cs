using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using EditorTools.Editor;
    using UniModules.Editor;
    using UniCore.Runtime.ReflectionUtils;
    using UnityEditor;
    using UnityEngine;

    public class GeneratedAsset<TAsset> : 
        ScriptableObject
        where TAsset : ScriptableObject
    {
        
        #region static data

        private static TAsset selector;

        public readonly static Type ProcessorType = typeof(TAsset);

        public static string AssetPath { get; private set; }

        public static TAsset Asset
        {
            get
            {
                if (selector) return selector;
                selector = CreateAsset();
                return selector;
            }
        }

        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            EditorApplication.delayCall += InnerInitialize;
        }

        private static void InnerInitialize()
        {
            AssetPath = EditorPathConstants.GeneratedContentDefaultPath.CombinePath($"GeneratedAsset/Editor/{ProcessorType.Namespace}/{ProcessorType.Name}");
            var asset = Asset;
        }

        private static TAsset CreateAsset()
        {
                            
            GameLog.Log($"GeneratedAsset Create asset of type {nameof(TAsset)} : with path : {AssetPath}");
                
            AssetDatabase.Refresh();
                
            var info = ProcessorType.GetCustomAttribute<GeneratedAssetInfoAttribute>();
            var path = info == null || string.IsNullOrEmpty(info.Location) ? AssetPath : 
                EditorPathConstants.GeneratedContentDefaultPath.CombinePath(info.Location);
                
            AssetPath = path;
                
            var newAsset  = AssetEditorTools.LoadOrCreate<TAsset>(ProcessorType, path);

            return newAsset;
        }
        
        #endregion

        public void Reset()
        {
            selector = null;
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Destroy()
        {
            if (!selector) return;
            var path = AssetDatabase.GetAssetPath(selector);
            AssetDatabase.DeleteAsset(path);
        }

    }
}