using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniModules.Editor;
using Object = UnityEngine.Object;

namespace UniModules.OdinTools.GameEditor.Base
{
    [Serializable]
    public class AssetsGroupCategory<TAsset> : GameEditorCategory<GameEditorConfiguration> where TAsset : Object
    {
        [InlineEditor]
        [Searchable]
        public List<TAsset> assets = new List<TAsset>();

        public override object CreateDrawer()
        {
            assets.Clear();
            assets.AddRange(AssetEditorTools.GetAssets<TAsset>());
            return this;
        }
    }
}
