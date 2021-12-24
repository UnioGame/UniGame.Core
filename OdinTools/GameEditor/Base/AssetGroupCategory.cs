using System;
using Sirenix.OdinInspector;
using UniModules.Editor;

namespace UniModules.OdinTools.GameEditor.Base
{
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class AssetGroupCategory<TAsset> : GameEditorCategory<GameEditorConfiguration> where TAsset : Object
    {
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        [HideLabel]
        public TAsset asset;

        public override object CreateDrawer()
        {
            asset = AssetEditorTools.GetAsset<TAsset>();
            return this;
        }
    }
}
