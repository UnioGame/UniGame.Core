using System;
using System.Collections.Generic;
using UniModules.UniCore.Runtime.ReflectionUtils;
using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.DataStructure
{
    using System.Linq;
    using global::UniCore.Runtime.ProfilerTools;

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : 
        Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver,
        ISerializableDictionaryObject
    {
        
        [SerializeField] protected List<TKey>   keys   = new List<TKey>();
        [SerializeField] protected List<TValue> values = new List<TValue>();

        #region constructor

        public SerializableDictionary(int capacity) : base(capacity) { }

        public SerializableDictionary() : base() { }

        #endregion
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count) {
                throw new System.Exception($"there are {typeof(TKey).GetFormattedName()}" + keys.Count + $" keys and {typeof(TValue).GetFormattedName()}" + values.Count +
                                           " values after deserialization. Make sure that both key and value types are serializable.");
            }

            for (var i = 0; i < keys.Count; i++) {
                try { 
                    this.Add(keys[i], values[i]);
                }
                catch (Exception e) {
                    GameLog.LogError($"{GetType().Name} {nameof(OnAfterDeserialize)} KEY {keys[i]} VALUE {values[i]} EXEP {e}");
                    throw;
                }
            }
            
        }
        
         
        protected virtual IEnumerable<TKey> GetKeys() => Enumerable.Empty<TKey>();
        
        
        protected virtual IEnumerable<TValue> GetValues() => Enumerable.Empty<TValue>();

    }
}