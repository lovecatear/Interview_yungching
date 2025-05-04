using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Models;

namespace ProductHub.Data.Contexts;

/// <summary>
/// Database context for the ProductHub application
/// </summary>
public class ProductHubContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ProductHubContext
    /// </summary>
    /// <param name="options">The options for this context</param>
    public ProductHubContext(DbContextOptions<ProductHubContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet for Product entities
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Configures the entity models and their relationships
    /// </summary>
    /// <param name="modelBuilder">The model builder instance</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });
    }
} 