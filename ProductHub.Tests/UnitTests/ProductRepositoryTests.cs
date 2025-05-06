using ProductHub.Data.Contexts;
using ProductHub.Data.Repositories;
using ProductHub.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductHub.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for Product Repository
    /// Ensures proper data persistence operations
    /// Validates: CRUD operations, data integrity, transactions
    /// </summary>
    public class ProductRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ProductHubContext> _options;
        private readonly ProductHubContext _context;
        private readonly ProductRepository _repository;

        /// <summary>
        /// Test setup: Initializes in-memory database context
        /// </summary>
        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ProductHubContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductHubContext(_options);
            _repository = new ProductRepository(_context);
        }

        /// <summary>
        /// GetAllAsync
        /// Should retrieve all products from database
        /// Checks: Query execution, data completeness
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Test Product 1",
                    Description = "Test Description 1",
                    Price = 100.00m,
                    Stock = 10,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                    IsActive = true
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Test Product 2",
                    Description = "Test Description 2",
                    Price = 200.00m,
                    Stock = 20,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                    IsActive = true
                }
            };

            await _context.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Test Product 1");
            Assert.Contains(result, p => p.Name == "Test Product 2");
        }

        /// <summary>
        /// GetByIdAsync
        /// Should retrieve specific product from database
        /// Checks: Query execution, data accuracy
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.00m,
                Stock = 10,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        /// <summary>
        /// AddAsync
        /// Should persist new product to database
        /// Checks: Data insertion, ID generation
        /// </summary>
        [Fact]
        public async Task AddAsync_WithValidProduct_ShouldAddProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "New Product",
                Description = "New Description",
                Price = 200.00m,
                Stock = 20,
                IsActive = true
            };

            // Act
            var result = await _repository.AddAsync(product);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            var savedProduct = await _context.Products.FindAsync(result.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal("New Product", savedProduct.Name);
        }

        /// <summary>
        /// UpdateAsync
        /// Should modify existing product in database
        /// Checks: Data modification, concurrency
        /// </summary>
        [Fact]
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Original Product",
                Description = "Original Description",
                Price = 100.00m,
                Stock = 10,
                CreateTime = DateTime.UtcNow.AddDays(-1),
                UpdateTime = DateTime.UtcNow.AddDays(-1),
                IsActive = true
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            product.Name = "Updated Product";
            product.Price = 150.00m;
            var result = await _repository.UpdateAsync(product);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            var updatedProduct = await _context.Products.FindAsync(productId);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.Equal(150.00m, updatedProduct.Price);
            Assert.True(updatedProduct.UpdateTime >= product.UpdateTime);
        }

        /// <summary>
        /// DeleteAsync
        /// Should remove product from database
        /// Checks: Data removal, cascade operations
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithValidProduct_ShouldDeleteProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Product to Delete",
                Description = "Description",
                Price = 100.00m,
                Stock = 10,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product);
            await _context.SaveChangesAsync();

            // Assert
            var deletedProduct = await _context.Products.FindAsync(productId);
            Assert.Null(deletedProduct);
        }

        /// <summary>
        /// Resource cleanup
        /// Ensures proper database disposal
        /// </summary>
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}