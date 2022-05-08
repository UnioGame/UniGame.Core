using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class ClassPoolExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DespawnClass<T>(this T data) where T : class, new()
        {
            ClassPool.Despawn(data);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DespawnWithRelease<T>(this T data) where T : class, new()
        {
            ClassPool.DespawnWithRelease(data);
        }
        
        public static void Despawn(this UnityEngine.Object data)
        {
#if UNITY_EDITOR
            if (data == null)
            {
                Debug.LogError("Try to return NULL value to the object pool");
                return;
            } 
#endif
            switch (data)
            {
                case GameObject gameObject:
                    gameObject.DespawnAsset();
                    return;
                case Component component:
                    component.gameObject.DespawnAsset();
                    return;
            }
        }
        
        public static void DespawnClass<T>(this T data, Action cleanupAction)
            where T: class, new()
        {
            ClassPool.Despawn(data,cleanupAction);
        }
        
        public static void DespawnRecursive<TValue,TData>(this TValue data)
            where TValue : class, ICollection<TData>, new() where TData : class, new()
        {
            DespawnItems(data);
            DespawnCollection<TValue,TData>(data);
        }
        
        public static void Despawn<TData>(this List<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this HashSet<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this Stack<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TKey,TValue>(this Dictionary<TKey,TValue> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this Queue<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void DespawnCollection<TData>(this List<TData> value)
            where  TData : class
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void DespawnCollection<TValue,TData>(this TValue value)
            where  TValue : class, ICollection<TData>, new()
        {
            value.Clear();
            ClassPool.Despawn(value);
        }

        public static void DespawnDictionary<TData,TKey,TValue>(this TData data)
            where TData : class, IDictionary<TKey,TValue>, new()
        {
            data.Clear();
            ClassPool.Despawn(data);
        }

        public static void DespawnItems<TData>(this ICollection<TData> data) where TData : class, new()
        {
            foreach (var item in data)
                ClassPool.DespawnWithRelease(item);
            
            data.Clear();
        }

    }
}
