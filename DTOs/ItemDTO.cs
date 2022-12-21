using Catalog.Domain;

namespace Catalog.DTOs;

public record ItemDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedAt { get; init; }

    public static ItemDTO fromEntity(Item item)
    {
        return new ItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            CreatedAt = item.CreatedAt,
        };
    }
}