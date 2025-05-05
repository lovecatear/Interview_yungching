using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Models;
using ProductHub.Data.Contexts;

namespace ProductHub.Data.Seeders;

/// <summary>
/// Seeder class for product data
/// </summary>
public static class ProductSeeder
{
    /// <summary>
    /// Initialize product data
    /// </summary>
    public static async Task SeedAsync(ProductHubContext context)
    {
        // Check if data already exists
        if (await context.Products.AnyAsync())
        {
            return;
        }

        // Create initial product data
        var products = new List<Product>
        {
            new()
            {
                Name = "iPhone 15 Pro",
                Description = "Apple's latest flagship phone with A17 Pro chip",
                Price = 35900,
                Stock = 100,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = "MacBook Pro 14",
                Description = "Professional laptop with M3 Pro chip",
                Price = 52900,
                Stock = 50,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = "AirPods Pro 2",
                Description = "Wireless earbuds with active noise cancellation",
                Price = 6990,
                Stock = 200,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = "iPad Air",
                Description = "Lightweight tablet for everyday use",
                Price = 18900,
                Stock = 75,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = "Apple Watch Series 9",
                Description = "Latest generation smartwatch",
                Price = 12900,
                Stock = 150,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            }
        };

        // Add data to database
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
} 