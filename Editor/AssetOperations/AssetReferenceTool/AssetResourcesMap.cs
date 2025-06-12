﻿namespace UniModules.Editor
{
    using System;
    using global::UniGame.DataStructure;
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