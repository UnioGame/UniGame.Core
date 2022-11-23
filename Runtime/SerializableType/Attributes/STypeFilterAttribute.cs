namespace UniGame.Core.Runtime.SerializableType.Attributes
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class STypeFilterAttribute : PropertyAttribute, ISerializedTypeFilter
    {
        public readonly Type   type;
        public readonly string fieldName;
        public readonly bool   useFilter;

        public STypeFilterAttribute(Type type,
            bool useFilter,
            string fieldName = nameof(SType.fullTypeName))
        {
            this.type      = type;
            this.useFilter = useFilter;
            this.fieldName = fieldName;
        }
        
        public STypeFilterAttribute(Type type, string fieldName = nameof(SType.fullTypeName))
        {
            this.type      = type;
            this.useFilter = false;
            this.fieldName = fieldName;
        }

        public Type Type => type;

        public string FieldName => fieldName;

        public bool UseFilter => useFilter;
    }
}
