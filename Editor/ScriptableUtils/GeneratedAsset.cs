namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using UniModules.Editor;
#endif
    
    public class GeneratedAsset<TAsset> : 
        ScriptableObject
        where TAsset : ScriptableObject
    {
#if UNITY_EDITOR
        
        #region static data

        private static TAsset selector;

        public static readonly Type ProcessorType = typeof(TAsset);

        public static string AssetPath =>  GeneratedTypeItem<TAsset>.AssetPath;

        public static TAsset Asset => GeneratedTypeItem<TAsset>.Asset;

        [InitializeOnLoadMethod]
        public static void InitializeByEditor()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                return;
#endif
            EditorApplication.delayCall += InnerInitialize;
        }

        private static void InnerInitialize()
        {
            var asset = Asset;
        }
        
        #endregion
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void DestroyAsset()
        {
            if (!selector) return;
            var path = AssetDatabase.GetAssetPath(selector);
            AssetDatabase.DeleteAsset(path);
            selector = null;
            this.MarkDirty();
        }

#endif
        
    }
}