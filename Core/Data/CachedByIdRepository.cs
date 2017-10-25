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
    /// in the cache individuality with a key created by 
    /// a pattern and their Id.
    /// </summary>
    /// <typeparam name="T">The type of the entity this repository exposes.</typeparam>
    public abstract class CachedByIdRepository<T> : IRepository<T> where T : class, IBaseEntity, IUID {
        #region Fields and Properties
        /// <summary>
        /// The pattern of the keys that the
        /// repository uses to store the data to the cache.
        /// </summary>
        protected abstract string Pattern { get; }

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
        public CachedByIdRepository(
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
        public async Task<T> Find(object id) {
            var key = GetKey(id);
            var entity = await _cacheManager.Get<T>(key);

            if (entity.IsNotNull())
                return entity;

            entity = await _repository.Find(id);
            await CacheEntity(entity);

            return entity;
        }

        /// <summary>
        /// Inserts a new entity and caches it.
        /// </summary>
        /// <param name="entity">The entity that will be inserted.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Insert(T entity) {
            var result = await _repository.Insert(entity);
            await CacheEntity(result.Entity);
            return result;
        }

        /// <summary>
        /// Updates an entity and re caches it.
        /// </summary>
        /// <param name="entity">The entity that will be updated.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Update(T entity) {
            var result = await _repository.Update(entity);
            await CacheEntity(result.Entity);
            return result;
        }

        /// <summary>
        /// Removes an entity and re caches the table data.
        /// </summary>
        /// <param name="entity">The entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Remove(T entity) {
            var result = await _repository.Remove(entity);
            if (result.Success)
                await _cacheManager.Remove(GetKey(entity.Id));
            return result;
        }

        /// <summary>
        /// Removes an entity by id and re caches the table data.
        /// </summary>
        /// <param name="id">The id of the entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        public async Task<RepositoryResult<T>> Remove(object id) {
            var result = await _repository.Remove(id);
            if (result.Success)
                await _cacheManager.Remove(GetKey(id));
            return result;
        }

        /// <summary>
        /// Saves changes for this table 
        /// and re caches the table data.
        /// </summary>
        public async Task Save() {
            await _repository.Save();
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
        /// Gets the key of an entity
        /// base on the pattern and the
        /// entity's id
        /// </summary>
        /// <param name="id">The Id of the entity.</param>
        /// <returns>the key of the entity.</returns>
        private string GetKey(object id)
            => Pattern.Format(id);
        
        /// <summary>
        /// Caches the entity.
        /// </summary>
        private async Task CacheEntity(T entity) {
            var key = GetKey(entity.Id);
            await _cacheManager.Set(key, entity, 60);
        }
        #endregion Private Methods
    }
}