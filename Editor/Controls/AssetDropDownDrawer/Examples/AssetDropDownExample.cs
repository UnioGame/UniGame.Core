namespace UniModules.Editor.Controls.AssetDropDownControl.Examples
{
    using UniGame.Core.Runtime.Attributes;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/Examples/AssetDropDown/AssetDropDownExample")]
    public class AssetDropDownExample : ScriptableObject
    {

        [AssetFilter]
        public DemoAssetDropDownAsset Item;

        [AssetFilter]
        public DemoAssetDropDownAsset Item2;
        
    }
}
