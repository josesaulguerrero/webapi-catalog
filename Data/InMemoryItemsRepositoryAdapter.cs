using Catalog.Domain;

namespace Catalog.Data;

public class InMemoryItemsRepositoryAdapter : ItemsRepositoryContract
{
    private readonly List<Item> items = new()
    {
        new Item {Id = Guid.NewGuid(), Name = "Potion", Price = 9.45M, CreatedAt = DateTimeOffset.Now},
        new Item {Id = Guid.NewGuid(), Name = "Iron Ore", Price = 30.56M, CreatedAt = DateTimeOffset.Now},
        new Item {Id = Guid.NewGuid(), Name = "Gold Armor", Price = 120.56M, CreatedAt = DateTimeOffset.Now},
    };

    public void DeleteItem(Guid id)
    {
        this.items.RemoveAll(item => item.Id == id);
    }

    public IEnumerable<Item> GetAllItems() => this.items;

    public Item GetItemById(Guid id) => this.items.SingleOrDefault(item => item.Id == id);

    public void SaveItem(Item item)
    {
        this.items.Add(item);
    }

    public void UpdateItem(Item item)
    {
        int itemIndex = this.items.FindIndex(i => i.Id == item.Id);
        items[itemIndex] = item;
    }
}