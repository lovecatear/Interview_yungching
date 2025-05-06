using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Models;
using ProductHub.Data.Contexts;
using ProductHub.Data.Interfaces;

namespace ProductHub.Data.Repositories;

/// <summary>
/// Implementation of the product repository interface.
/// Handles data access operations for products using Entity Framework Core.
/// </summary>
public class ProductRepository(ProductHubContext context) : IProductRepository
{
    private readonly ProductHubContext _context = context;

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>A collection of all products.</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Retrieves a paged list of products with filtering, sorting, and searching capabilities.
    /// </summary>
    /// <param name="parameters">The query parameters for filtering, sorting, and pagination.</param>
    /// <returns>A paged result containing the filtered and sorted products.</returns>
    public async Task<PagedResult<Product>> GetPagedAsync(ProductQueryParameters parameters)
    {
        var query = _context.Products.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var searchTerm = parameters.SearchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                p.Description.ToLower().Contains(searchTerm));
        }

        // Apply active status filter
        if (parameters.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == parameters.IsActive.Value);
        }

        // Apply price range filter
        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= parameters.MinPrice.Value);
        }
        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= parameters.MaxPrice.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = parameters.SortBy.ToLower() switch
        {
            "name" => parameters.SortOrder == "desc" 
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "price" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "stock" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(p => p.Stock)
                : query.OrderBy(p => p.Stock),
            "createtime" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(p => p.CreateTime)
                : query.OrderBy(p => p.CreateTime),
            "updatetime" => parameters.SortOrder == "desc"
                ? query.OrderByDescending(p => p.UpdateTime)
                : query.OrderBy(p => p.UpdateTime),
            _ => query.OrderBy(p => p.Name)
        };

        // Apply pagination
        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);

        return new PagedResult<Product>
        {
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = items
        };
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product if found.</returns>
    /// <exception cref="ArgumentException">Thrown when the product is not found.</exception>
    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new ArgumentException($"Product with ID {id} not found");
        return product;
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <returns>The added product with its generated ID and timestamps.</returns>
    public async Task<Product> AddAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreateTime = DateTime.UtcNow;
        product.UpdateTime = DateTime.UtcNow;

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    /// <param name="product">The product with updated information.</param>
    /// <returns>The updated product.</returns>
    /// <exception cref="ArgumentException">Thrown when the product is not found.</exception>
    public async Task<Product> UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null)
            throw new ArgumentException($"Product with ID {product.Id} not found");

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.IsActive = product.IsActive;
        existingProduct.UpdateTime = DateTime.UtcNow;

        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    /// <returns>True if the product was deleted successfully; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null)
            return false;

        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync();
        return true;
    }
}