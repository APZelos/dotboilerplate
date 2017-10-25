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
        public async Task<T> Get<T>(string key) => (T)Cache[key];

        /// <summary>
        /// Gets all the values stored in the cache
        /// that their keys match the given pattern.
        /// All values must be of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the stored values.</typeparam>
        /// <param name="pattern">The pattern that the keys must match.</param>
        /// <returns>an enumerable that contains all the values stored in the cache that their keys match the given pattern.</returns>
        public async Task<IEnumerable<T>> GetByPattern<T>(string pattern) {
            var regex = pattern.ToSingleLineCaseInsensitiveRegex(true);
            return Cache
                .Where(item => regex.IsMatch(item.Key))
                .Select(item => (T)item.Value);
        }

        /// <summary>
        /// Checks if any value is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key that is going to be checked against the cache.</param>
        /// <returns>true if any value is stored in the cache with the given key.</returns>
        public async Task<bool> IsSet(string key) => Cache.Contains(key);

        /// <summary>
        /// Removes the value that is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key of the value that is going to be removed.</param>
        public async Task Remove(string key) => Cache.Remove(key);

        /// <summary>
        /// Removes the value that is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key of the value that is going to be removed.</param>
        public async Task RemoveByPattern(string pattern) {
            var regex = pattern.ToSingleLineCaseInsensitiveRegex(true);
            var keysToRemove = Cache
                .Where(item => regex.IsMatch(item.Key))
                .Select(item => item.Key);
            foreach(var key in keysToRemove) {
                await Remove(key);
            }
        }

        /// <summary>
        /// Sets the value with the given key to the cache.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        public async Task Set(string key, object value, int time) {
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
        public async Task<bool> SetIfNotExist(string key, object value, int time) {
            if (value == null) return false;
            if (await IsSet(key)) return false;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(time);
            Cache.Add(new CacheItem(key, value), policy);
            return true;
        }

        /// <summary>
        /// Clears values stored in the cache.
        /// </summary>
        public async Task Clear() {
            foreach (var item in Cache)
                await Remove(item.Key);
        }
    }
}
