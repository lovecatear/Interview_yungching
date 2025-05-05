namespace ProductHub.Common.Models;

/// <summary>
/// Represents a product in the system
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
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