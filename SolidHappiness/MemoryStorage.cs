using System;
using System.Collections.Generic;

namespace SolidHappiness
{
    internal class MemoryStorage : IStorage
    {
        private readonly Dictionary<string, object> _storage;

        internal MemoryStorage()
        {
            _storage = new Dictionary<string, object>();
        }

        public bool Exists(string key)
        {
            return _storage.ContainsKey(key);
        }

        public object Get(string key)
        {
            if (!_storage.ContainsKey(key))
                return null;

            return _storage[key];
        }

        public void Set(string key, object value)
        {
            if (!_storage.ContainsKey(key))
                _storage.Add(key, null);

            _storage[key] = value;
        }
    }
}