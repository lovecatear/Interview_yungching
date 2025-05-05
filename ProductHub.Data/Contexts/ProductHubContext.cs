using Microsoft.EntityFrameworkCore;
using ProductHub.Common.Models;
using ProductHub.Data.Seeders;

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
            // Primary key
            entity.HasKey(e => e.Id);

            // Required fields
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired();

            // Decimal precision
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            // Integer constraints
            entity.Property(e => e.Stock)
                .IsRequired();

            // DateTime defaults
            entity.Property(e => e.CreateTime)
                .IsRequired();

            entity.Property(e => e.UpdateTime)
                .IsRequired();

            // Boolean default
            entity.Property(e => e.IsActive)
                .IsRequired();

            // Indexes
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
        });
    }

    /// <summary>
    /// Executes when database is created
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Execute seeder on first save
        if (ChangeTracker.HasChanges())
        {
            await ProductSeeder.SeedAsync(this);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
} 