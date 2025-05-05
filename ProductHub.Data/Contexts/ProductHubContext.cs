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
            // Primary key with auto-generated Guid
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            // Required fields
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired();

            // Decimal precision
            entity.Property(e => e.Price)
                .HasPrecision(18, 2);

            // Integer constraints
            entity.Property(e => e.Stock)
                .IsRequired();

            // DateTime defaults
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("GETUTCDATE()");

            // Boolean default
            entity.Property(e => e.IsActive)
                .IsRequired();

            // Indexes
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
        });
    }
} 