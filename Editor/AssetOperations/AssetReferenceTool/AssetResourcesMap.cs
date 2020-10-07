namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations.AssetReferenceTool
{
    using System;
    using EditorResources;
    using UniModules.UniGame.Core.Runtime.DataStructure;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetResourcesMap : SerializableDictionary<Object,ResourceHandle>
    {
        public AssetResourcesMap(int capacity)
            : base(capacity)
        {
            
        }
    }
}