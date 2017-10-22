using System.Linq;

namespace Core.Data {
    /// <summary>
    /// The main repository interface that 
    /// all repositories must implement.
    /// </summary>
    /// <typeparam name="T">The type of the entity that this repository exposes.</typeparam>
    public interface IRepository<T> where T : IBaseEntity {
        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">The id of the entity that will be returned.</param>
        /// <returns>The entity with the given id.</returns>
        T Find(object id);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">The entity that will be inserted.</param>
        /// <returns>a RepositoryResult.</returns>
        RepositoryResult<T> Insert(T entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity that will be updated.</param>
        /// <returns>a RepositoryResult.</returns>
        RepositoryResult<T> Update(T entity);

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        RepositoryResult<T> Remove(T entity);

        /// <summary>
        /// Removes an entity by id.
        /// </summary>
        /// <param name="id">The id of the entity that will be removed.</param>
        /// <returns>a RepositoryResult.</returns>
        RepositoryResult<T> Remove(object id);

        /// <summary>
        /// Exposes an object that allows querying 
        /// the table of repository's entity type.
        /// </summary>
        IQueryable<T> Query { get; }

        /// <summary>
        /// Saves changes for this table.
        /// </summary>
        void Save();

        /// <summary>
        /// Loads relationship objects for this entity.
        /// </summary>
        /// <param name="entity">The entity we want to load relationships for.</param>
        void Load(T entity);

        /// <summary>
        /// Attaches entity to the context if is not attached.
        /// </summary>
        /// <param name="entity">The entity we want to attach to the context.</param>
        void Attach(T entity);

        /// <summary>
        /// Dettaches entity from the context.
        /// </summary>
        /// <param name="entity">The entity we want to dettach from the context.</param>
        void Dettach(T entity);
    }
}