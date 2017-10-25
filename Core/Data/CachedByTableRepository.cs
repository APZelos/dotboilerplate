using Core.Cache;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
    /// <summary>
    /// The repositories that inherit from this class
    /// store all the data of the table that they expose
    /// in the cache as an enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the entity this repository exposes.</typeparam>
    public abstract class CachedByTableRepository<T> : IRepository<T> where T : class, IBaseEntity, IUID {
        #region Fields and Properties
        /// <summary>
        /// The key that indicates the 
        /// stored table data in the cache.
        /// </summary>
        protected abstract string TableKey { get; }

        /// <summary>
        /// The query that selects the records that will be stored in the cache.
        /// </summary>
        protected Func<IQueryable<T>, IEnumerable<T>> QueryForCache
            => (table) => table;

        /// <summary>
        /// The cache manager that will be 
        /// used to store and get the data.
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// The repository that will be used
        /// to get the data from the DB.
        /// </summary>
        private readonly IRepository<T> _repository;
        #endregion Fields and Properties

        /// <summary>
        /// The default ctor.
        /// </summary>
        public CachedByTableRepository(
            ICacheManager cacheManager,
            IRepository<T> repository) {
            _cacheManager = cacheManager;
            _repository = repository;
        }

        #region Public Methods
        /// <summary>
        /// Executes a query on the table of repository's entity type.
        /// This method always gets data from the DB.
        /// </summary>
        public async Task<IEnumerable<T>> Query(Func<IQueryable<T>, IEnumerable<T>> query)
            => await _repository.Query(query);
        
        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">The id of the entity that will be returned.</param>
        /// <returns>The entity with the given id.</returns>
        public async Task<T> Find(object id) 
            => (await GetCachedTable())?
            .FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// Inserts a new entity and re caches the table data.
        /// </summary>
        /// <param name="entity">The entity that will be inserted.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Insert(T entity) {
            var result = await _repository.Insert(entity);
            await CacheTable();
            return result;
        }

        /// <summary>
        /// Updates an entity and re caches the table data.
        /// </summary>
        /// <param name="entity">The entity that will be updated.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Update(T entity) {
            var result = await _repository.Update(entity);
            await CacheTable();
            return result;
        }

        /// <summary>
        /// Removes an entity and re caches the table data.
        /// </summary>
        /// <param name="entity">The entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Remove(T entity) {
            var result = await _repository.Remove(entity);
            await CacheTable();
            return result;
        }

        /// <summary>
        /// Removes an entity by id and re caches the table data.
        /// </summary>
        /// <param name="id">The id of the entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Remove(object id) {
            var result = await _repository.Remove(id);
            await CacheTable();
            return result;
        }

        /// <summary>
        /// Saves changes for this table 
        /// and re caches the table data.
        /// </summary>
        public async Task Save() {
            await _repository.Save();
            await CacheTable();
        }

        /// <summary>
        /// Attaches entity to the context if is not attached.
        /// </summary>
        /// <param name="entity">The entity we want to attach to the context.</param>
        public async Task Attach(T entity)
            => await _repository.Attach(entity);

        /// <summary>
        /// Detaches entity from the context.
        /// </summary>
        /// <param name="entity">The entity we want to detach from the context.</param>
        public async Task Dettach(T entity)
            => await _repository.Dettach(entity);
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Gets table data from cache.
        /// If no data is set in cache, it caches the data.
        /// </summary>
        /// <returns>an enumerable of the cached table data</returns>
        private async Task<IEnumerable<T>> GetCachedTable() {
            IEnumerable<T> table;

            if (!(await _cacheManager.IsSet(TableKey)))
                table = await CacheTable();
            else
                table = await _cacheManager.Get<IEnumerable<T>>(TableKey);

            return table;
        }

        /// <summary>
        /// Caches the table data. If they are already cached it
        /// removes the data from the cache and re caches them.
        /// </summary>
        /// <returns>an enumerable with the data that were cached.</returns>
        private async Task<IEnumerable<T>> CacheTable() {
            if (await _cacheManager.IsSet(TableKey))
                await _cacheManager.Remove(TableKey);

            var table = (await _repository.Query(QueryForCache)).ToList();
            await _cacheManager.Set(TableKey, table, 60);

            return table;
        }
        #endregion Private Methods
    }
}
