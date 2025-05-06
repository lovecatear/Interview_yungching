using ProductHub.Business.Interfaces;
using ProductHub.Common.Models;
using ProductHub.Data.Interfaces;

namespace ProductHub.Business.Services;

/// <summary>
/// Implementation of the product service interface
/// </summary>
public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>A collection of all products</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a paged list of products with filtering, sorting, and searching
    /// </summary>
    /// <param name="parameters">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A paged result of products</returns>
    public async Task<PagedResult<Product>> GetPagedAsync(ProductQueryParameters parameters)
    {
        // Validate parameters
        if (parameters.PageNumber < 1)
            parameters.PageNumber = 1;

        if (parameters.PageSize < 1)
            parameters.PageSize = 10;

        // Validate price range
        if (parameters.MinPrice.HasValue && parameters.MaxPrice.HasValue &&
            parameters.MinPrice.Value > parameters.MaxPrice.Value)
        {
            throw new ArgumentException("Minimum price cannot be greater than maximum price");
        }

        return await _productRepository.GetPagedAsync(parameters);
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product with its ID</returns>
    public async Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreateTime = DateTime.UtcNow;
        product.UpdateTime = DateTime.UtcNow;
        product.IsActive = true;
        return await _productRepository.AddAsync(product);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="product">The product to update</param>
    /// <returns>The updated product if found, null otherwise</returns>
    public async Task<Product> UpdateAsync(Guid id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new ArgumentException($"Product with ID {id} not found");

        product.Id = id;
        product.CreateTime = existingProduct.CreateTime;
        product.UpdateTime = DateTime.UtcNow;
        return await _productRepository.UpdateAsync(product);
    }

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>True if the product was deleted, false otherwise</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return false;

        return await _productRepository.DeleteAsync(product);
    }

    /// <summary>
    /// Checks if a product exists by its ID
    /// </summary>
    /// <param name="id">The ID to check</param>
    /// <returns>True if the product exists, false otherwise</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null;
    }
}