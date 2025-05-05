using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductHub.Data.Contexts;

public class ProductHubContextFactory : IDesignTimeDbContextFactory<ProductHubContext>
{
    public ProductHubContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductHubContext>();
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=ProductHub;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

        return new ProductHubContext(optionsBuilder.Options);
    }
} 