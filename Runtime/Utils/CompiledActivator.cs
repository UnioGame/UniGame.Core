

namespace UniModules.UniCore.Runtime.Utils
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    
    public static class CompiledActivator
    {

        private static readonly MemorizeItem<Type, ConstructorInfo> getDefaultConstructor = MemorizeTool.Memorize<Type, ConstructorInfo>(
            x =>
            {
                var ctor = x.GetConstructor(Type.EmptyTypes);
                return ctor;
            });

        private static readonly MemorizeItem<Type, Delegate> lambdaActivator = MemorizeTool.Memorize<Type, Delegate>(type =>
        {
            //get default constructor
            var ctor = getDefaultConstructor[type];
            if (ctor == null) 
                return null;
            
            // Make a NewExpression that calls the ctor with the args we just created
            var newExp = Expression.New(ctor);                  

            // Create a lambda with the New expression as body and our param object[] as arg
            var lambda = Expression.Lambda(newExp);

            var delegateObject = lambda.Compile();
            
            return delegateObject;
        });

        #region extensions
        
                
        public static bool HasDefaultConstructor(this Type type)
        {
            var cnstr = getDefaultConstructor[type];
            return cnstr != null;
        }

        public static object CreateWithDefaultConstructor(this Type type) => CreateInstance(type);

        public static TType CreateWithDefaultConstructor<TType>(this Type type)
            where TType : class => CreateInstance<TType>(type);

        #endregion

        public static object CreateInstance(Type type) => lambdaActivator[type]?.DynamicInvoke();

        public static TType CreateInstance<TType>(Type type) where TType : class => CreateInstance(type) as TType;


    }
}