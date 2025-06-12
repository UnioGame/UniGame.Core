﻿namespace UniGame.Runtime.Utils
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class TypeToStringConverter
    {
        private static readonly Func<int, string> IntConverter = MemorizeTool.Create<int,string>(x => x.ToString());
        private static readonly Func<uint, string> UIntConverter = MemorizeTool.Create<uint, string>(x => x.ToString());
        private static readonly Func<float, string> FloatConverter = MemorizeTool.Create<float, string>(x => x.ToString());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringFromCache(this int value)
        {
            return IntConverter.Invoke(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringFromCache(this bool value)
        {
            var convertedValue = value ? 1 : 0;
            return convertedValue.ToStringFromCache();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringRoundToInt(this float value)
        {
            return Mathf.RoundToInt(value).ToStringFromCache();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringFromCache<T>(this T value)
            where  T : Enum
        {
            return EnumValue<T>.GetName(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringFromCache(this float value)
        {
            return FloatConverter.Invoke(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToStringFromCache(this uint value)
        {
            return UIntConverter.Invoke(value);
        }
    }
}
