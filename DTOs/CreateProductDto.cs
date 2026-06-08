using System.ComponentModel.DataAnnotations;
using StorageApi.Constants;

namespace StorageApi.DTOs;

public sealed class CreateProductDto
{
    [Required]
    [StringLength(
        ProductValidationConstants.NameMaximumLength,
        MinimumLength = ProductValidationConstants.NameMinimumLength
    )]
    public string Name { get; set; } = string.Empty;

    [Range(
        ProductValidationConstants.MinimumPrice,
        ProductValidationConstants.MaximumPrice
    )]
    public int Price { get; set; }

    [Required]
    [StringLength(
        ProductValidationConstants.CategoryMaximumLength,
        MinimumLength = ProductValidationConstants.CategoryMinimumLength
    )]
    public string Category { get; set; } = string.Empty;

    [Required]
    [StringLength(
        ProductValidationConstants.ShelfMaximumLength,
        MinimumLength = ProductValidationConstants.ShelfMinimumLength
    )]
    public string Shelf { get; set; } = string.Empty;

    [Range(
        ProductValidationConstants.MinimumCount,
        ProductValidationConstants.MaximumCount
    )]
    public int Count { get; set; }

    [StringLength(ProductValidationConstants.DescriptionMaximumLength)]
    public string? Description { get; set; } = string.Empty;
}
