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
        Task<T> Get<T>(string key);

        /// <summary>
        /// Gets all the values stored in the cache
        /// that their keys match the given pattern.
        /// All values must be of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the stored values.</typeparam>
        /// <param name="pattern">The pattern that the keys must match.</param>
        /// <returns>an enumerable that contains all the values stored in the cache that their keys match the given pattern.</returns>
        Task<IEnumerable<T>> GetByPattern<T>(string pattern);

        /// <summary>
        /// Sets the value with the given key to the cache.
        /// If a value with the same key already exist in the
        /// cache it removes it and sets the new value.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        Task Set(string key, object value, int time);

        /// <summary>
        /// Sets the value with the given key to the cache.
        /// If a value with the same key already exist in the
        /// cache it the new value is rejected.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        /// <returns>true if no value was found with the same key in the cache and the given value was added.</returns>
        Task<bool> Add(string key, object value, int time);

        /// <summary>
        /// Sets the value with the given key to the cache
        /// if no other values is already stored in the cache
        /// with the same key.
        /// </summary>
        /// <param name="key">The key that the given value will be stored with.</param>
        /// <param name="value">The value that will be stored with the given key.</param>
        /// <param name="time">The time in minutes that the value will be stored in cache.</param>
        /// <returns>true if the value was stored in the cache.</returns>
        Task<bool> SetIfNotExist(string key, object value, int time);

        /// <summary>
        /// Checks if any value is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key that is going to be checked against the cache.</param>
        /// <returns>true if any value is stored in the cache with the given key.</returns>
        Task<bool> IsSet(string key);

        /// <summary>
        /// Removes the value that is stored in the cache
        /// with the given key.
        /// </summary>
        /// <param name="key">The key of the value that is going to be removed.</param>
        Task Remove(string key);

        /// <summary>
        /// Removes all the values stored in the cache
        /// that their key match the given pattern.
        /// </summary>
        /// <param name="pattern">The pattern that the keys must start with.</param>
        Task RemoveByPattern(string pattern);

        /// <summary>
        /// Clears values stored in the cache.
        /// </summary>
        Task Clear();
    }
}
