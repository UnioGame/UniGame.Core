namespace UniModules.UniGame.Core.Runtime.SerializableType.Attributes
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class STypeFilterAttribute : PropertyAttribute
    {
        public readonly Type   Type;
        public readonly string FieldName;
        public readonly bool   UseFilter;

        public STypeFilterAttribute(Type type,
            bool useFilter,
            string fieldName = nameof(SType.fullTypeName))
        {
            this.Type      = type;
            this.UseFilter = useFilter;
            this.FieldName = fieldName;
        }
        
        public STypeFilterAttribute(Type type,
            string fieldName = nameof(SType.fullTypeName))
        {
            this.Type      = type;
            this.UseFilter = false;
            this.FieldName = fieldName;
        }
    }
}
