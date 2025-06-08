namespace UniModules.Editor
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.DataStructure;
    using Object = UnityEngine.Object;

    [Serializable]
    public class AssetReferencesMap : SerializableDictionary<Object, List<ResourceHandle>>
    {
        public AssetReferencesMap(int capacity)
            : base(capacity)
        {
            
        }
    }
}