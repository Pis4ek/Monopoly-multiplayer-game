using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRx
{
    public class ReactiveDictionary<TKey, TValue> :  IEnumerable
    {
        public event Action<KeyValuePair<TKey, TValue>> OnItemAdded;
        public event Action<KeyValuePair<TKey, TValue>> OnItemRemoved;
        public event Action<KeyValuePair<TKey, TValue>> OnItemChanged;
        public event Action OnCountChanged;
        public event Action OnCleared;

        private Dictionary<TKey, TValue> _dictionary;

        #region CONSTRUCTORS
        public ReactiveDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }
        public ReactiveDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }
        public ReactiveDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public ReactiveDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }
        #endregion


        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                TValue oldValue;
                if (TryGetValue(key, out oldValue))
                {
                    _dictionary[key] = value;
                    OnItemChanged?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
                }
                else
                {
                    _dictionary[key] = value;
                    OnCountChanged?.Invoke();
                    OnItemAdded?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnCountChanged?.Invoke();
            OnItemAdded?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
            OnCountChanged?.Invoke();
            OnItemAdded?.Invoke(item);
        }

        public bool Remove(TKey key)
        {
            TValue value;
            TryGetValue(key, out value);

            if (_dictionary.Remove(key)) 
            {
                OnCountChanged?.Invoke();
                OnItemRemoved?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionary.Remove(item.Key))
            {
                OnCountChanged?.Invoke();
                OnItemRemoved?.Invoke(item);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            List<KeyValuePair<TKey, TValue>> removedPairs = new List<KeyValuePair<TKey, TValue>>(Count);
            foreach (var item in _dictionary)
            {
                removedPairs.Add(item);
            }
            _dictionary.Clear();
            OnCleared?.Invoke();

            foreach (var item in removedPairs)
            {
                OnItemRemoved?.Invoke(item);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }
    }
}