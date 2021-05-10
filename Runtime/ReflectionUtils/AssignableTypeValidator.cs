using System;
using UniModules.UniCore.Runtime.Utils;

namespace UniGame.Core.Runtime.ReflectionUtils
{
    
    public static class AssignableTypeValidator<TSource>
    {
        #region static data
        
        private static MemorizeItem<Type, bool> isAssignableTypeMethod = MemorizeTool.Memorize<Type, bool>(x => SourceType.IsAssignableFrom(x));
        
        public static readonly Type SourceType = typeof(TSource);

        #endregion
        
        public static bool Validate(Type type)
        {
            return isAssignableTypeMethod[type];
        }
    }

}