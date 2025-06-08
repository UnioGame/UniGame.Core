namespace UniModules.GameEditor.Categories
{
    using System;
    using global::UniGame.Core.Editor;
    using UnityEngine;

    [Serializable]
    public class AutoAssetCategoryView<TAsset> : ViewCategory<TAsset>, IAutoEditorCategory
        where TAsset : ScriptableObject
    {
        public override TAsset CreateView()
        {
            return ValueTypeCache.LoadAsset<TAsset>();
        }
    }
}