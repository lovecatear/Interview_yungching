namespace ProductHub.Common.Models;

/// <summary>
/// Base class for all entities, providing common properties
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Creation timestamp of the entity
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Last update timestamp of the entity
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Active status of the entity
    /// </summary>
    public bool IsActive { get; set; } = true;
}