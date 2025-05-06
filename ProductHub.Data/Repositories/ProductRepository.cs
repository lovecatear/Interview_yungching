using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Models;
using ProductHub.Data.Contexts;
using ProductHub.Data.Interfaces;

namespace ProductHub.Data.Repositories;

public class ProductRepository(ProductHubContext context) : IProductRepository
{
    private readonly ProductHubContext _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            throw new ArgumentException($"Product with ID {id} not found");
        return product;
    }

    public async Task<Product> AddAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreateTime = DateTime.UtcNow;
        product.UpdateTime = DateTime.UtcNow;

        await _context.Products.AddAsync(product);
        return product;
    }

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
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null)
            return false;

        _context.Products.Remove(existingProduct);
        return true;
    }
}