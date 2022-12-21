using System.ComponentModel.DataAnnotations;

namespace Catalog.DTOs;

public record CreateItemDTO
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "The field {0} is required; null values or empty strings are not allowed.")]
    public string Name { get; init; }

    [DataType(DataType.Currency)]
    [Required(ErrorMessage = "The field {0} is required; null values are not allowed either.")]
    [Range(minimum: 1.0, maximum: double.MaxValue, ErrorMessage = "{0} must be greater than {1} and lesser than {2}")]
    public decimal Price { get; init; }
}