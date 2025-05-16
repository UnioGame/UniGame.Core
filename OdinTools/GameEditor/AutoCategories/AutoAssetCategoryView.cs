namespace UniModules.GameEditor.Categories
{
    using System;
    using UniGame.Core.Editor.EditorProcessors;
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