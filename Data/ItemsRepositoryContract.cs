using Catalog.Domain;

namespace Catalog.Data;

public interface ItemsRepositoryContract
{
    public Task<IEnumerable<Item>> GetAllItemsAsync();

    public Task<Item> GetItemByIdAsync(Guid id);

    public Task SaveItemAsync(Item item);

    public Task UpdateItemAsync(Item item);

    public Task DeleteItemAsync(Guid id);
}