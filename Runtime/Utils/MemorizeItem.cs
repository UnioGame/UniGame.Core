namespace UniModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MemorizeItem<TKey, TData> : IDisposable
    {
        private readonly Func<TKey, TData> factory;
        private readonly Action<TData>     disposableAction;
        private Dictionary<TKey,TData> _cache = new Dictionary<TKey,TData>(16);

        public MemorizeItem(Action<TData> disposableAction = null)
        {
            this.factory          = null;
            this.disposableAction = disposableAction;
        }
        
        public MemorizeItem(Func<TKey, TData> factory, Action<TData> disposableAction = null)
        {
            this.factory          = factory;
            this.disposableAction = disposableAction;
        }

        public TData this[TKey x]
        {
            get =>  GetValue(x);
            set => _cache[x] = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key) => _cache.ContainsKey(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TData value) => _cache.TryGetValue(key, out value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key) => _cache.Remove(key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TData GetValue(TKey key)
        {
            if (_cache.TryGetValue(key, out var value) && value != null) 
                return value;

            if (factory == null) return default;
            
            value       = factory(key);
            _cache[key] = value;
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (disposableAction != null) {
                foreach (var data in _cache) {
                    disposableAction.Invoke(data.Value);
                }
            }
            _cache.Clear();
        }
    }
}