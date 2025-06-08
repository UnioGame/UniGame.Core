using System.Collections.Generic;
using UnityEngine;

namespace UniGame.Runtime.Utils
{
    using System;

    public static class MemorizeTool
    {
        private static Action cleanupStream;
        private static List<IDisposable> cacheItems = new List<IDisposable>();

        public static MemorizeItem<TKey,TData> Memorize<TKey,TData>(Func<TKey, TData> factory, Action<TData> disposableAction = null)
        {
            var cache = new MemorizeItem<TKey,TData>(factory,disposableAction);
            
#if UNITY_EDITOR
            //clean up cache if Assembly Reload
            cacheItems.Add(cache);
#endif
            return cache;
        }

        public static Func<TKey,TData> Create<TKey,TData>(Func<TKey,TData> factory, Action<TData> disposableAction = null) {

            var cache = Memorize(factory,disposableAction);

            return cache.GetValue;
        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnDomainReload()
        {
            Application.quitting -= OnQuitting;
            Application.quitting += OnQuitting;
            CleanUp();
        }

        public static void CleanUp()
        {
            foreach (var item in cacheItems)
                item.Dispose();
            cacheItems.Clear();
        }

        private static void OnQuitting()
        {
            Application.quitting -= OnQuitting;
        }
#endif
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("UniGame/Tools/Clear Memorize Cache")]
        public static void Clear() => CleanUp();
#endif
        
    }
}
