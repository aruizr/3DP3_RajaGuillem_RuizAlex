using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

namespace Utilities.Misc
{
    [Serializable]
    public class SerializableDictionary<TK, TV> : Dictionary<TK, TV>, ISerializationCallbackReceiver
    {
        [SerializeField] private KeyValue[] dictionary;

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Clear();
            dictionary.ForEach(kv =>
            {
                if (!ContainsKey(kv.key)) Add(kv.key, kv.value);
            });
        }

        [Serializable]
        private struct KeyValue
        {
            public TK key;
            public TV value;

            public KeyValue(KeyValuePair<TK, TV> pair)
            {
                key = pair.Key;
                value = pair.Value;
            }
        }
    }
}