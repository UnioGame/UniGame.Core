namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using UniModules.Editor;
#endif
    
    public abstract class GeneratedAsset<TAsset> : 
        ScriptableObject
        where TAsset : ScriptableObject
    {
#if UNITY_EDITOR
        
        #region static data

        private static TAsset _selector;

        public static readonly Type AssetType = typeof(TAsset);

        public static string AssetPath =>  GeneratedTypeItem<TAsset>.AssetPath;

        public static TAsset Asset => GeneratedTypeItem<TAsset>.Asset;
        
        public static void Load(Action<TAsset> action)
        {
            GeneratedTypeItem<TAsset>.Load(x => Load(x, action));
        }
        
        private static void Load(TAsset asset, Action<TAsset> action)
        {
            _selector = asset;
            action?.Invoke(asset);
        }
        
        #endregion
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void DestroyAsset()
        {
            if (!_selector) return;
            var path = AssetDatabase.GetAssetPath(_selector);
            AssetDatabase.DeleteAsset(path);
            _selector = null;
            this.MarkDirty();
        }

#endif
        
    }
}