using ProductHub.Common.Models;

namespace ProductHub.Data.Interfaces;

/// <summary>
/// Defines the contract for product data access operations.
/// This interface provides methods for CRUD operations and advanced querying capabilities.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products from the data store.
    /// </summary>
    /// <returns>A collection of all products.</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Retrieves a paged list of products with filtering, sorting, and searching capabilities.
    /// </summary>
    /// <param name="parameters">The query parameters for filtering, sorting, and pagination.</param>
    /// <returns>A paged result containing the filtered and sorted products.</returns>
    Task<PagedResult<Product>> GetPagedAsync(ProductQueryParameters parameters);

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    /// <returns>The product if found.</returns>
    /// <exception cref="ArgumentException">Thrown when the product is not found.</exception>
    Task<Product> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new product to the data store.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <returns>The added product with its generated ID and timestamps.</returns>
    Task<Product> AddAsync(Product product);

    /// <summary>
    /// Updates an existing product in the data store.
    /// </summary>
    /// <param name="product">The product with updated information.</param>
    /// <returns>The updated product.</returns>
    /// <exception cref="ArgumentException">Thrown when the product is not found.</exception>
    Task<Product> UpdateAsync(Product product);

    /// <summary>
    /// Deletes a product from the data store.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    /// <returns>True if the product was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteAsync(Product product);
}