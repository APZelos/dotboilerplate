using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache {
    /// <summary>
    /// The main CacheManager.
    /// </summary>
    public class CacheManager : ICacheManager {

        /// <summary>
        /// The cache object.
        /// </summary>
        protected ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        /// Gets the value stored with the given key.
        /// </summary>
        /// <typeparam name="T">Type of the stored value.</typeparam>
        /// <param name="key">The key the value is stored with.</param>
        /// <returns>the value stored with the given key.</returns>
        public T Get<T>(string key) => (T)Cache[key];

        /// <summary>
        /// Gets all the values stored in the cache
        /// that their keys starts with the given pattern.
        /// All values must be of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the stored values.</typeparam>
        /// <param name="pattern">The pattern that the keys must start with.</param>
        /// <returns>an enumerable that contains all the values stored in the cache that their keys starts with the given pattern.</returns>
        public IEnumerable<T> GetByPattern<T>(string pattern) {
            var regex = pattern.ToSingleLineCaseInsensitiveRegex(true);
            return Cache
                .Where(item => regex.IsMatch(item.Key))
                .Select(item => (T)item.Value);
        }

        /// <summary>
        /// Tries to get the value stored with the given key.
        /// </summary>
        /// <typeparam name="T">Type of the stored value.</typeparam>
        /// <param name="key">The key the value is stored with.</param>
        /// <param name="value">A variable that will hold the value stored with the given key, if any exist.</param>
        /// <returns>true if any value was found stored with the given key.</returns>
        public bool TryGet<T>(string key, out T value) {
            value = default(T);
            if (!IsSet(key)) return false;
            value = Get<T>(key);
            return true;
        }

        public bool TryGetByPattern<T>(string pattern, out IEnumerable<T> values) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if any value is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key that is going to be checked against the cache.</param>
        /// <returns>true if any value is stored in the cache with the given key.</returns>
        public bool IsSet(string key) => Cache.Contains(key);

        /// <summary>
        /// Removes the value that is stored in the cache
        /// with the the given key.
        /// </summary>
        /// <param name="key">The key of the value that is going to be removed.</param>
        public void Remove(string key) => Cache.Remove(key);

        public void RemoveByPattern(string pattern) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the value with the given key to the cache.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        public void Set(string key, object value, int time) {
            if (value == null) return;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(time);
            Cache.Add(new CacheItem(key, value), policy);
        }

        /// <summary>
        /// Sets the value with the given key to the cache
        /// if no other values is already stored in the cache
        /// with the same key.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        /// <returns>true if the value was stored in the cache.</returns>
        public bool SetIfNotExist(string key, object value, int time) {
            if (value == null) return false;
            if (IsSet(key)) return false;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(time);
            Cache.Add(new CacheItem(key, value), policy);
            return true;
        }

        /// <summary>
        /// Clears values stored in the cache.
        /// </summary>
        public void Clear() {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}
