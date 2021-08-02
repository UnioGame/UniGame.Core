namespace UniModules.Editor
{
    using System;
    using System.Collections.Generic;
    using UniModules.UniGame.Core.Runtime.DataStructure;
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