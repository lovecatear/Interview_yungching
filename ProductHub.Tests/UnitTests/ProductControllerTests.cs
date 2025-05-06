using Moq;
using ProductHub.Server.Controllers;
using ProductHub.Business.Interfaces;
using ProductHub.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductHub.Tests.UnitTests
{
    /// <summary>
    /// Unit tests for Product Controller
    /// Ensures proper handling of HTTP requests and responses
    /// Validates: Status codes, response formats, input validation
    /// </summary>
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;

        /// <summary>
        /// Test setup: Initializes controller with mocked service
        /// </summary>
        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        /// <summary>
        /// GET: api/products
        /// Should return OK with product list
        /// Checks: Response type, data completeness
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldReturnOkResultWithProducts()
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

            _mockProductService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(expectedProducts.Count, returnValue.Count);
            Assert.Equal(expectedProducts[0].Name, returnValue[0].Name);
        }

        /// <summary>
        /// GET: api/products/{id}
        /// Should return OK with specific product
        /// Checks: Response type, product data, ID handling
        /// </summary>
        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOkResult()
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

            _mockProductService.Setup(service => service.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnValue.Id);
            Assert.Equal(expectedProduct.Name, returnValue.Name);
        }

        /// <summary>
        /// POST: api/products
        /// Should return Created with new product
        /// Checks: Response type, product data, location header
        /// </summary>
        [Fact]
        public async Task Create_WithValidData_ShouldReturnCreatedResult()
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

            _mockProductService.Setup(service => service.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // Act
            var result = await _controller.Create(newProduct);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal(newProduct.Name, returnValue.Name);
            Assert.Equal(newProduct.Price, returnValue.Price);
        }

        /// <summary>
        /// PUT: api/products/{id}
        /// Should return OK with updated product
        /// Checks: Response type, update confirmation
        /// </summary>
        [Fact]
        public async Task Update_WithValidData_ShouldReturnOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updatedProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150.00m,
                Stock = 15,
                IsActive = true,
                UpdateTime = DateTime.UtcNow
            };

            _mockProductService.Setup(service => service.ExistsAsync(productId))
                .ReturnsAsync(true);

            _mockProductService.Setup(service => service.UpdateAsync(productId, It.IsAny<Product>()))
                .ReturnsAsync(updatedProduct);

            _mockProductService.Setup(service => service.GetByIdAsync(productId))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.Update(productId, updatedProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = okResult.Value;
            Assert.NotNull(returnValue);
            Assert.Equal("Product updated successfully", returnValue.GetType().GetProperty("message")?.GetValue(returnValue));
            Assert.NotNull(returnValue.GetType().GetProperty("updatedAt")?.GetValue(returnValue));
        }

        /// <summary>
        /// DELETE: api/products/{id}
        /// Should return OK with deletion confirmation
        /// Checks: Response type, operation success
        /// </summary>
        [Fact]
        public async Task Delete_WithValidId_ShouldReturnOkResult()
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

            _mockProductService.Setup(service => service.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _mockProductService.Setup(service => service.DeleteAsync(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
} 