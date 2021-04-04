namespace Taktika.GameRuntime.Models.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    [Serializable]
    public class ProxySerializableDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        ISerializationCallbackReceiver
    {
        [SerializeField] protected List<TKey> keys = new List<TKey>();
        [SerializeField] protected List<TValue> values = new List<TValue>();
        private IDictionary<TKey, TValue> _dictionaryImplementation = new Dictionary<TKey, TValue>();

        public ProxySerializableDictionary(IDictionary<TKey, TValue> innerDictionary) : base()
        {
            _dictionaryImplementation = innerDictionary;
        }

        public IDictionary<TKey, TValue> GetDictionary() => _dictionaryImplementation;

        public void OnBeforeSerialize()
        {
            if (_dictionaryImplementation == null)
                _dictionaryImplementation = new Dictionary<TKey, TValue>();
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            if (_dictionaryImplementation == null)
                _dictionaryImplementation = new Dictionary<TKey, TValue>();
            this.Clear();
            _dictionaryImplementation.Clear();
            if (keys.Count != values.Count)
            {
                throw new System.Exception("there are " + keys.Count + " keys and " + values.Count +
                                           " values after deserialization. Make sure that both key and value types are serializable.");
            }

            for (var i = 0; i < keys.Count; i++)
            {
                try
                {
                    this.Add(keys[i], values[i]);
                }
                catch (Exception e)
                {
                    GameLog.LogError($"{GetType().Name} {nameof(OnAfterDeserialize)} KEY {keys[i]} VALUE {values[i]} EXEP {e}");
                    throw;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionaryImplementation.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionaryImplementation).GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionaryImplementation.Add(item);
        }

        public void Clear()
        {
            _dictionaryImplementation.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionaryImplementation.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionaryImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionaryImplementation.Remove(item);

        public int Count => _dictionaryImplementation.Count;

        public bool IsReadOnly => _dictionaryImplementation.IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            _dictionaryImplementation.Add(key, value);
        }

        public bool ContainsKey(TKey key) => _dictionaryImplementation.ContainsKey(key);

        public bool Remove(TKey key) => _dictionaryImplementation.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => _dictionaryImplementation.TryGetValue(key, out value);

        public TValue this[TKey key]
        {
            get => _dictionaryImplementation[key];
            set => _dictionaryImplementation[key] = value;
        }

        public ICollection<TKey> Keys => _dictionaryImplementation.Keys;

        public ICollection<TValue> Values => _dictionaryImplementation.Values;

    }
}