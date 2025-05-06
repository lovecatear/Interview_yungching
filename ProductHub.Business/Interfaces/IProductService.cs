using ProductHub.Common.Models;

namespace ProductHub.Business.Interfaces;

/// <summary>
/// Service interface for product-related business operations
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Product?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>A collection of all products</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product with its ID</returns>
    Task<Product> CreateAsync(Product product);

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="product">The product to update</param>
    /// <returns>The updated product</returns>
    Task<Product> UpdateAsync(Guid id, Product product);

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>True if the product was deleted, false otherwise</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a product exists by its ID
    /// </summary>
    /// <param name="id">The ID to check</param>
    /// <returns>True if the product exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id);
}