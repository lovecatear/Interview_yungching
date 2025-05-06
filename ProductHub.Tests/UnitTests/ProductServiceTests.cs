using Moq;
using ProductHub.Business.Interfaces;
using ProductHub.Data.Interfaces;
using ProductHub.Common.Models;
using ProductHub.Business.Services;

namespace ProductHub.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for Product Service
    /// Ensures proper business logic implementation
    /// Validates: Data operations, business rules, error handling
    /// </summary>
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IProductService _productService;

        /// <summary>
        /// Test setup: Initializes service with mocked repository
        /// </summary>
        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        /// <summary>
        /// GetAllAsync
        /// Should return complete product collection
        /// Checks: Data retrieval, collection integrity
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Test Product",
                    Description = "Test Description",
                    Price = 100.00m,
                    Stock = 10,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                    IsActive = true
                }
            };

            _mockProductRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.Equal(expectedProducts.Count, result.Count());
            Assert.Equal(expectedProducts[0].Name, result.First().Name);
        }

        /// <summary>
        /// GetByIdAsync
        /// Should return correct product by ID
        /// Checks: Data retrieval, ID validation
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
                Stock = 10,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                IsActive = true
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);
        }

        /// <summary>
        /// CreateAsync
        /// Should create new product with valid data
        /// Checks: Data persistence, property validation
        /// </summary>
        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "New Product",
                Description = "New Description",
                Price = 200.00m,
                Stock = 20,
                IsActive = true
            };

            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // Act
            var result = await _productService.CreateAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(newProduct.Name, result.Name);
            Assert.True(result.CreateTime > DateTime.UtcNow.AddMinutes(-1));
        }

        /// <summary>
        /// UpdateAsync
        /// Should update existing product with valid data
        /// Checks: Data modification, timestamp handling
        /// </summary>
        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
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

            var updatedProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150.00m,
                Stock = 15,
                IsActive = true
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // Act
            var result = await _productService.UpdateAsync(productId, updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.True(result.UpdateTime > existingProduct.UpdateTime);
        }

        /// <summary>
        /// DeleteAsync
        /// Should remove product from system
        /// Checks: Data removal, operation success
        /// </summary>
        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteProduct()
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
                IsActive = true
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockProductRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteAsync(productId);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// ExistsAsync
        /// Should confirm product existence
        /// Checks: Data verification, ID validation
        /// </summary>
        [Fact]
        public async Task ExistsAsync_WithValidId_ShouldReturnCorrectResult()
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
                IsActive = true
            };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.ExistsAsync(productId);

            // Assert
            Assert.True(result);
        }
    }
} 