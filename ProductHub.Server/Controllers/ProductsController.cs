using Microsoft.AspNetCore.Mvc;
using ProductHub.Business.Interfaces;
using ProductHub.Common.Models;

namespace ProductHub.Server.Controllers;

/// <summary>
/// Controller for handling product-related HTTP requests
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Retrieves all products from the system
    /// </summary>
    /// <returns>A list of all active products</returns>
    /// <response code="200">Returns the list of products</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        try
        {
            var products = await productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving products", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <returns>The requested product if found</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="400">If the product ID is invalid</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid product ID" });
            }

            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }
            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the product", error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new product in the system
    /// </summary>
    /// <param name="product">The product data to create</param>
    /// <returns>The newly created product with its generated ID</returns>
    /// <response code="201">Returns the newly created product</response>
    /// <response code="400">If the product data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid product data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            product.IsActive = true;
            var createdProduct = await productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the product", error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing product in the system
    /// </summary>
    /// <param name="id">The unique identifier of the product to update</param>
    /// <param name="product">The updated product data</param>
    /// <returns>The updated product information</returns>
    /// <response code="200">Returns the updated product information</response>
    /// <response code="400">If the product data is invalid or IDs don't match</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Update(Guid id, [FromBody] Product product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid product data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid product ID" });
            }

            if (id != product.Id)
            {
                return BadRequest(new { message = "Product ID mismatch" });
            }

            if (!await productService.ExistsAsync(id))
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            await productService.UpdateAsync(product);
            var updatedProduct = await productService.GetByIdAsync(id);
            return Ok(new
            {
                message = "Product updated successfully",
                updatedAt = updatedProduct?.UpdateTime
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the product", error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a product from the system
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <returns>Information about the deleted product</returns>
    /// <response code="200">Returns the deletion confirmation</response>
    /// <response code="400">If the product ID is invalid</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid product ID" });
            }

            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            await productService.DeleteAsync(id);
            return Ok(new
            {
                message = "Product deleted successfully",
                deletedProductId = id,
                deletedProductName = product.Name,
                deletedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the product", error = ex.Message });
        }
    }
}