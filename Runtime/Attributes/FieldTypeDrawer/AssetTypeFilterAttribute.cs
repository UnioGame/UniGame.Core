using System;
using UnityEngine;

namespace UniGame.Attributes.FieldTypeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssetTypeFilterAttribute : PropertyAttribute
    {
        public Type Type;
        
        public AssetTypeFilterAttribute(Type assetType)
        {
            Type = assetType;
        }
    }
}
