namespace UniGame.Core.Runtime.SerializableType.Extensions
{
    using System;
    using System.Collections.Generic;
    using UniModules.UniCore.Runtime.ReflectionUtils;
    using UniModules.UniCore.Runtime.Utils;

    public static class STypeExtensions
    {
        public static MemorizeItem<Type,List<SType>> TypeCache = 
            MemorizeTool.Memorize<Type,List<SType>>(FilterByTypeNonCached);

        public static List<SType> GetAssignableNonAbstractTypes(this SType baseType)
        {
            var types = TypeCache[baseType];
            return types;
        }

        public static List<SType> FilterByTypeNonCached(Type baseType)
        {
            var types = baseType.GetAssignableTypes(true);
            var items = new List<SType>();
            foreach (var type in types)
                items.Add(type);
            return items;
        }
    }
}