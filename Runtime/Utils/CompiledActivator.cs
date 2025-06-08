namespace UniGame.Runtime.Utils
{
    using System.Linq;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using UnityEngine;

    public static class CompiledActivator
    {
        public static readonly MemorizeItem<Type, Type[]> _constructorTypes =
            MemorizeTool.Memorize<Type, Type[]>(
                x =>
                {
                    var defaultConstructor = x.GetConstructor(Type.EmptyTypes);
                    if (defaultConstructor!=null)
                        return Type.EmptyTypes;
                    
                    var constructor = x
                        .GetConstructors(BindingFlags.Public)
                        .FirstOrDefault();
                    
                    if(constructor == null)
                        return Type.EmptyTypes;
                    
                    var parameters = constructor.GetParameters();
                    var types = parameters
                        .Select(info => info.ParameterType)
                        .ToArray();
                    return types;
                });
        
        private static readonly MemorizeItem<Type, ConstructorInfo> _getDefaultConstructor = MemorizeTool.Memorize<Type, ConstructorInfo>(
            x =>
            {
                var parameters = _constructorTypes[x];
                var ctor = x.GetConstructor(parameters);
                return ctor;
            });

        private static readonly MemorizeItem<Type, Delegate> _lambdaDefaultActivator = MemorizeTool.Memorize<Type, Delegate>(type =>
        {
            //get default constructor
            var ctor = _getDefaultConstructor[type];
            if (ctor == null) return null;
            
            // Make a NewExpression that calls the ctor with the args we just created
            var newExp = Expression.New(ctor);                  

            // Create a lambda with the New expression as body and our param object[] as arg
            var lambda = Expression.Lambda(newExp);

            var delegateObject = lambda.Compile();
            
            return delegateObject;
        });

       
        #region extensions

        /// <summary>
        /// get parameters of type. If have default constructor then return Type.EmptyTypes or parameters of first public constructor
        /// </summary>
        /// <param name="type">source type</param>
        /// <returns>array of constructor parameters</returns>
        public static Type[] GetParameters(this Type type) => _constructorTypes[type];
                
        public static bool HasDefaultConstructor(this Type type)
        {
            var cnstr = _getDefaultConstructor[type];
            return cnstr != null;
        }

        public static object CreateWithDefaultConstructor(this Type type)
        {
            return type.HasDefaultConstructor() == false ? null : CreateInstance(type);
        }

        public static TType CreateWithDefaultConstructor<TType>(this Type type)
            where TType : class => CreateWithDefaultConstructor(type) as TType;
        
        
        public static object CreateObject(this Type type)
        {
            if (type.HasDefaultConstructor())
                return type.CreateWithDefaultConstructor();
            return null;
        }

        #endregion

        public static object CreateInstance(Type type, params object[] args) =>
            _lambdaDefaultActivator[type]?.DynamicInvoke(args);


    }

    
    
}