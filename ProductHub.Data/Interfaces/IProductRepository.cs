using ProductHub.Common.Models;

namespace ProductHub.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(Guid id);

        Task<Product> AddAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task<bool> DeleteAsync(Product product);
    }
} 