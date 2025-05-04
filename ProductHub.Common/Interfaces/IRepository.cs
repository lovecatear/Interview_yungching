using System.Linq.Expressions;
using ProductHub.Common.Models;

namespace ProductHub.Common.Interfaces;

/// <summary>
/// Generic repository interface defining basic CRUD operations
/// </summary>
/// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve</param>
    /// <returns>The entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all entities
    /// </summary>
    /// <returns>A collection of all entities</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Finds entities matching the specified predicate
    /// </summary>
    /// <param name="predicate">The search criteria</param>
    /// <returns>A collection of matching entities</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The added entity with its ID</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>A task representing the update operation</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Soft deletes an entity by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <returns>A task representing the delete operation</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if an entity exists by its ID
    /// </summary>
    /// <param name="id">The ID to check</param>
    /// <returns>True if the entity exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id);
}