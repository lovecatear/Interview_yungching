using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Interfaces;
using ProductHub.Common.Models;
using ProductHub.Data.Contexts;

namespace ProductHub.Data.Repositories;

/// <summary>
/// Generic repository implementation for database operations
/// </summary>
/// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
public class Repository<T>(ProductHubContext context) : IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Database context instance
    /// </summary>
    protected readonly ProductHubContext _context = context;

    /// <summary>
    /// DbSet for the entity type
    /// </summary>
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    /// <summary>
    /// Retrieves an entity by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve</param>
    /// <returns>The entity if found, null otherwise</returns>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all entities
    /// </summary>
    /// <returns>A collection of all entities</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Finds entities matching the specified predicate
    /// </summary>
    /// <param name="predicate">The search criteria</param>
    /// <returns>A collection of matching entities</returns>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The added entity with its ID</returns>
    public virtual async Task<T> AddAsync(T entity)
    {
        entity.CreateTime = DateTime.UtcNow;
        entity.UpdateTime = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>A task representing the update operation</returns>
    public virtual async Task UpdateAsync(T entity)
    {
        entity.UpdateTime = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Soft deletes an entity by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <returns>A task representing the delete operation</returns>
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsActive = false;
            await UpdateAsync(entity);
        }
    }

    /// <summary>
    /// Checks if an entity exists by its ID
    /// </summary>
    /// <param name="id">The ID to check</param>
    /// <returns>True if the entity exists, false otherwise</returns>
    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}