using System;
using UniModules.Editor;

namespace UniModules.GameEditor.Categories
{
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class AssetViewCategory<TAsset> : ViewCategory<TAsset>
        where TAsset : Object
    {
        public override TAsset CreateView()
        {
            return AssetEditorTools.GetAsset<TAsset>();
        }
    }
}
