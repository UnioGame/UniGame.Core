using System;
using System.Linq.Expressions;

namespace UniModules.UniCore.Runtime.Utils
{
    public static class CompiledActivator
    {

        private static MemorizeItem<Type, Delegate> lambdaActivator = MemorizeTool.Memorize<Type, Delegate>(type =>
        {
            //get default constructor
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null) 
                return null;
            
            // Make a NewExpression that calls the ctor with the args we just created
            var newExp = Expression.New(ctor);                  

            // Create a lambda with the New expression as body and our param object[] as arg
            var lambda = Expression.Lambda(newExp);

            var delegateObject = lambda.Compile();
            
            return delegateObject;
        });
        
        public static object CreateInstance(Type type)
        {
            return lambdaActivator[type]?.DynamicInvoke();
        }

        public static TType CreateInstance<TType>(Type type) 
            where TType : class => CreateInstance(type) as TType;


        public static object CreateWithDefaultConstructor(this Type type) => CreateInstance(type);
        
        public static TType CreateWithDefaultConstructor<TType>(this Type type)
            where TType : class => CreateInstance<TType>(type);

    }
}