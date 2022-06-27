using UnityEngine.Profiling;

namespace UniModules.UniGame.Core.Runtime.DataStructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public class SerializableReferenceDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver
    {
        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ValueDropdown(nameof(GetKeys))]
#endif
        protected List<TKey>   keys   = new List<TKey>();
        
        
        [SerializeReference] 
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ValueDropdown(nameof(GetValues))]
#endif
        protected List<TValue> values = new List<TValue>();

        #region construcotr

        public SerializableReferenceDictionary(int capacity) : base(capacity) { }

        public SerializableReferenceDictionary() : base() { }

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

            if (keys.Count != values.Count)
                throw new System.Exception("there are " + keys.Count + " keys and " + values.Count +
                                           " values after deserialization. Make sure that both key and value types are serializable.");

            for (var i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
            
        }
        
        protected virtual IEnumerable<TKey> GetKeys() => Enumerable.Empty<TKey>();
        
        
        protected virtual IEnumerable<TValue> GetValues() => Enumerable.Empty<TValue>();
        
        
    }
}