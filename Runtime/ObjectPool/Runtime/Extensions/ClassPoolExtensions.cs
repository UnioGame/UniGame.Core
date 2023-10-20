using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace UniGame.Runtime.ObjectPool.Extensions
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static class ClassPoolExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Despawn<T>(this T data) where T : class, new()
        {
            if (data is UnityEngine.Object asset)
            {
                Despawn(asset);
                return;
            }
            ClassPool.Despawn(data);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Despawn<T>(this T data,ref T value, T defaultValue = null) where T : class, new()
        {
            ClassPool.Despawn(data);
            value = defaultValue;
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
                default:
                    Object.Destroy(data);
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

#if NET_STANDARD
        
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Despawn<TData>(this TData[] value)
        {
            if (value == null) return;
            for (int i = 0; i < value.Length; i++)
                value[i] = default;
            
            ArrayPool<TData>.Shared.Return(value);
        }
        
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Despawn<TData>(this List<TData> value)
        {
            value.Clear();
            ListPool<TData>.Release(value);
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
            DictionaryPool<TKey,TValue>.Release(value);
        }
        
        public static void Despawn<TData>(this Queue<TData> value)
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

        public static void DespawnItems<TData>(this ICollection<TData> data) where TData : class, new()
        {
            foreach (var item in data)
                ClassPool.DespawnWithRelease(item);
            
            data.Clear();
        }

    }
}
