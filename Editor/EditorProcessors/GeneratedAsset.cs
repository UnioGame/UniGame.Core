namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using EditorTools.Editor;
    using EditorTools.Editor.AssetOperations;
    using EditorTools.Editor.Tools;
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

        public static string AssetPath { get; private set; } =
            EditorPathConstants.GeneratedContentDefaultPath.CombinePath($"GeneratedAsset/Editor/{ProcessorType.Namespace}/{ProcessorType.Name}");

        public static TAsset Asset
        {
            get
            {
                if (selector)
                    return selector;
                
                AssetDatabase.Refresh();
                
                var info = ProcessorType.GetCustomAttribute<GeneratedAssetInfoAttribute>();
                var path = info == null || string.IsNullOrEmpty(info.Location) ? AssetPath : 
                    EditorPathConstants.GeneratedContentDefaultPath.CombinePath(info.Location);
                
                AssetPath = path;
                
                selector  = AssetEditorTools.LoadOrCreate<TAsset>(ProcessorType, path);
                return selector;
            }
        }

        public void Reset()
        {
            selector = null;
        }

        public void Destroy()
        {
            if (!selector) return;
            var path = AssetDatabase.GetAssetPath(selector);
            AssetDatabase.DeleteAsset(path);
        }

        #endregion

    }
}