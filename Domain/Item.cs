namespace Catalog.Domain;

public class Item
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public decimal Price { get; set; }

    public DateTimeOffset CreatedAt { get; init; }
}