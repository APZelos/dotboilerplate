using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache {
    /// <summary>
    /// All CacheManagers must implement
    /// this interface.
    /// </summary>
    public interface ICacheManager {
        /// <summary>
        /// Gets the value stored with the given key.
        /// </summary>
        /// <typeparam name="T">Type of the stored value.</typeparam>
        /// <param name="key">The key the value is stored with.</param>
        /// <returns>the value stored with the given key.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Tries to get the value stored with the given key.
        /// </summary>
        /// <typeparam name="T">Type of the stored value.</typeparam>
        /// <param name="key">The key the value is stored with.</param>
        /// <param name="value">A variable that will hold the value stored with the given key, if any exist.</param>
        /// <returns>true if any value was found stored with the given key.</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Gets all the values stored in the cache
        /// that their keys starts with the given pattern.
        /// All values must be of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the stored values.</typeparam>
        /// <param name="pattern">The pattern that the keys must start with.</param>
        /// <returns>an enumerable that contains all the values stored in the cache that their keys starts with the given pattern.</returns>
        IEnumerable<T> GetByPattern<T>(string pattern);

        /// <summary>
        /// Tries to get all the values stored in the cache
        /// that their keys starts with the given pattern.
        /// All values must be of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the stored values.</typeparam>
        /// <param name="pattern">The pattern that the keys must start with.</param>
        /// <param name="values">A variable that will hold the values stored with the given pattern, if any exist.</param>
        /// <returns>true if any value was found stored with the given key.</returns>
        bool TryGetByPattern<T>(string pattern, out IEnumerable<T> values);

        /// <summary>
        /// Sets the value with the given key to the cache.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        void Set(string key, object value, int time);

        /// <summary>
        /// Sets the value with the given key to the cache
        /// if no other values is already stored in the cache
        /// with the same key.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        /// <returns>true if the value was stored in the cache.</returns>
        bool SetIfNotExist(string key, object value, int time);

        /// <summary>
        /// Checks if any value is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key that is going to be checked against the cache.</param>
        /// <returns>true if any value is stored in the cache with the given key.</returns>
        bool IsSet(string key);

        /// <summary>
        /// Removes the value that is stored in the cache
        /// with the the given key.
        /// </summary>
        /// <param name="key">The key of the value that is going to be removed.</param>
        void Remove(string key);

        /// <summary>
        /// Removes all the values stored in the cache
        /// that their key starts with the given pattern.
        /// </summary>
        /// <param name="pattern">The pattern that the keys must start with.</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clears values stored in the cache.
        /// </summary>
        void Clear();
    }
}
