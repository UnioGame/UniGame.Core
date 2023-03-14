namespace UniModules.Editor
{
    using System;
    using Object = UnityEngine.Object;

    public struct AttributeItemData<TAttribute>
        where TAttribute : Attribute
    {
        public static readonly AttributeItemData<TAttribute> Empty = new AttributeItemData<TAttribute>();
        
        public bool IsFound;
        public object    Source;
        public TAttribute Attribute;
        public object    Target;
    }
        
    public struct AttributeItemData<TValue,TAttribute>
        where TAttribute : Attribute
    {
        public TValue     Value;
        public TAttribute Attribute;
        public object    Target;
    }
}