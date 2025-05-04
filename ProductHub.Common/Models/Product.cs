namespace ProductHub.Common.Models;

/// <summary>
/// Represents a product in the system
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Current stock quantity of the product
    /// </summary>
    public int Stock { get; set; }
}