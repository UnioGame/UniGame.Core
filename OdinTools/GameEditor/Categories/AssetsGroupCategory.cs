using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.Editor;
using Object = UnityEngine.Object;

namespace UniModules.GameEditor.Categories
{
    [Serializable]
    public class AssetsGroupCategory<TAsset> : ViewCategory<List<TAsset>>
        where TAsset : Object
    {
        [InlineEditor]
        [Searchable]
        public List<TAsset> assets = new List<TAsset>();

        public override List<TAsset> CreateView()
        {
            assets.Clear();
            assets.AddRange(AssetEditorTools.GetAssets<TAsset>());
            return assets;
        }
    }
}
