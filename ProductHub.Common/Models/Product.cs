using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductHub.Common.Models;

/// <summary>
/// Represents a product in the system
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product description
    /// </summary>
    [Required(ErrorMessage = "Product description is required")]
    [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product price
    /// </summary>
    [Required(ErrorMessage = "Product price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Product price cannot be negative")]
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
    [Required(ErrorMessage = "Product stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Product stock cannot be negative")]
    public int Stock { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Product active status
    /// </summary>
    public bool IsActive { get; set; } = true;
}