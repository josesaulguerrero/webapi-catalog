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

    public async Task DeleteItemAsync(Guid id) => await Task.Run(() => this.items.RemoveAll(item => item.Id == id));

    public async Task<IEnumerable<Item>> GetAllItemsAsync() => await Task.Run(() => this.items);

    public async Task<Item> GetItemByIdAsync(Guid id) => await Task.Run(() => this.items.SingleOrDefault(item => item.Id == id));

    public async Task SaveItemAsync(Item item) => await Task.Run(() => this.items.Add(item));

    public async Task UpdateItemAsync(Item item)
    {
        await Task.Run(() =>
        {
            int itemIndex = this.items.FindIndex(i => i.Id == item.Id);
            items[itemIndex] = item;
        });
    }
}