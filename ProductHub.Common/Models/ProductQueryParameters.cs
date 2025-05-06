using System.ComponentModel.DataAnnotations;

namespace ProductHub.Common.Models;

/// <summary>
/// Represents the query parameters for product listing operations.
/// Provides support for pagination, sorting, filtering, and searching.
/// </summary>
public class ProductQueryParameters
{
    private const int MaxPageSize = 50;
    private const string PageSizeErrorMessage = "Page size must be between 1 and 50";
    private int _pageSize = 10;
    private string _searchTerm = string.Empty;
    private string _sortBy = "Name";
    private string _sortOrder = "asc";

    /// <summary>
    /// Gets or sets the page number for pagination (1-based indexing).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// The value is capped at <see cref="MaxPageSize"/>.
    /// </summary>
    [Range(1, MaxPageSize, ErrorMessage = PageSizeErrorMessage)]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, MaxPageSize);
    }

    /// <summary>
    /// Gets or sets the search term for filtering products by name or description.
    /// The search is case-insensitive and supports partial matches.
    /// </summary>
    public string SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the field to sort by.
    /// Supported values: Name, Price, Stock, CreateTime, UpdateTime.
    /// Defaults to "Name" if an invalid value is provided.
    /// </summary>
    public string SortBy
    {
        get => _sortBy;
        set => _sortBy = value?.Trim() ?? "Name";
    }

    /// <summary>
    /// Gets or sets the sort order.
    /// Valid values: "asc" or "desc" (case-insensitive).
    /// Defaults to "asc" if an invalid value is provided.
    /// </summary>
    public string SortOrder
    {
        get => _sortOrder;
        set => _sortOrder = value?.ToLower() == "desc" ? "desc" : "asc";
    }

    /// <summary>
    /// Gets or sets the active status filter.
    /// When set, only products matching this status will be returned.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the minimum price filter.
    /// When set, only products with price greater than or equal to this value will be returned.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Minimum price cannot be negative")]
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Gets or sets the maximum price filter.
    /// When set, only products with price less than or equal to this value will be returned.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Maximum price cannot be negative")]
    public decimal? MaxPrice { get; set; }
} 