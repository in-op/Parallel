using System;
using System.Collections.Generic;

namespace ParallelNET35.Concurrent
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class ConcurrentDictionary<TKey, TValue>
    {
        private readonly object locker = new object();
        private Dictionary<TKey, TValue> dict;

        /// <summary>
        /// Gets the number of key/value pairs contained within.
        /// </summary>
        public int Count
        {
            get
            {
                lock (locker)
                {
                    return dict.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the dictionary is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                lock (locker)
                {
                    return dict.Count > 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                lock (locker)
                {
                    return dict[key];
                }

            }

            set
            {
                lock (locker)
                {
                    dict[key] = value;
                }

            }
        }

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public void Clear()
        {
            lock (locker)
            {
                dict.Clear();
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            lock (locker)
            {
                return dict.ContainsKey(key);
            }
        }

        /// <summary>
        /// Copies the key and value pairs stored in the dictionary to a new array.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<TKey, TValue>[] ToArray()
        {
            lock (locker)
            {
                var output = new KeyValuePair<TKey, TValue>[dict.Count];
                int i = 0;
                foreach (KeyValuePair<TKey, TValue> kvp in dict)
                {
                    output[i] = kvp;
                    i++;
                }
                return output;
            }
        }

        /// <summary>
        /// Attempts to add the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if the key/value pair was added to the dictionary successfully. False if the key already exists.</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            lock (locker)
            {
                if (dict.ContainsKey(key)) return false;
                dict.Add(key, value);
                return true;
            }
        }


    }
}
