using ProductHub.Business.Interfaces;
using ProductHub.Common.Interfaces;
using ProductHub.Common.Models;

namespace ProductHub.Business.Services;

/// <summary>
/// Implementation of the product service interface
/// </summary>
public class ProductService(IRepository<Product> repository) : IProductService
{
    /// <summary>
    /// Repository instance for product data access
    /// </summary>
    private readonly IRepository<Product> _repository = repository;

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>A collection of all products</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product with its ID</returns>
    public async Task<Product> CreateAsync(Product product)
    {
        return await _repository.AddAsync(product);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">The product to update</param>
    /// <returns>A task representing the update operation</returns>
    public async Task UpdateAsync(Product product)
    {
        await _repository.UpdateAsync(product);
    }

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>A task representing the delete operation</returns>
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Checks if a product exists by its ID
    /// </summary>
    /// <param name="id">The ID to check</param>
    /// <returns>True if the product exists, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _repository.ExistsAsync(id);
    }
}