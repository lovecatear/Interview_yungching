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
    /// Retrieves all products
    /// </summary>
    /// <returns>A list of all products</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// Retrieves a specific product by ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The product if found, NotFound if not found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product with its ID</returns>
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        var createdProduct = await productService.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="product">The updated product data</param>
    /// <returns>NoContent if successful, BadRequest if IDs don't match, NotFound if product doesn't exist</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!await productService.ExistsAsync(id))
        {
            return NotFound();
        }

        await productService.UpdateAsync(product);
        return NoContent();
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>NoContent if successful, NotFound if product doesn't exist</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await productService.ExistsAsync(id))
        {
            return NotFound();
        }

        await productService.DeleteAsync(id);
        return NoContent();
    }
}
