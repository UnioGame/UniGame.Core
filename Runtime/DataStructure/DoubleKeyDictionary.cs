﻿namespace UniGame.Runtime.DataStructure
{
    using System.Collections.Generic;

    public class DoubleKeyDictionary<TKey,TSubKey,TValue>
    {

        protected Dictionary<TKey, Dictionary<TSubKey, TValue>> _dictionary =
            new Dictionary<TKey, Dictionary<TSubKey, TValue>>();

        public IReadOnlyCollection<TKey> Keys => _dictionary.Keys;

        public IReadOnlyDictionary<TSubKey,TValue> this[TKey key]
        {
            get
            {
                if (!_dictionary.TryGetValue(key, out var items))
                {
                    return null;
                }
                return items;
            }
        }

        public void Clear()
        {
            foreach (var item in _dictionary) {
                item.Value.Clear();
            }
            _dictionary.Clear();
        }

        public bool Contains(TKey key, TSubKey value)
        {
            if (!_dictionary.TryGetValue(key, out var items))
                return false;

            return items.ContainsKey(value);
        }

        public TValue Get(TKey key, TSubKey subKey)
        {
            if (!_dictionary.TryGetValue(key, out var nodeContexts))
            {
                return default(TValue);
            }

            if (!nodeContexts.TryGetValue(subKey, out var data))
            {
                return default(TValue);
            }

            return data;
        }

        public void Add(TKey key, TSubKey subKey,TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var subStates))
            {
                subStates = new Dictionary<TSubKey, TValue>();
                _dictionary[key] = subStates;
            }

            subStates[subKey] = value;

        }

        public virtual bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public virtual bool Remove(TKey key, TSubKey subKey)
        {
            if (!_dictionary.TryGetValue(key, out var nodeContexts))
            {
                return false;
            }
            return nodeContexts.Remove(subKey);
        }

    }
}
