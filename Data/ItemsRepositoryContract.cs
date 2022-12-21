using Catalog.Domain;

namespace Catalog.Data;

public interface ItemsRepositoryContract
{
    public IEnumerable<Item> GetAllItems();

    public Item GetItemById(Guid id);

    public void SaveItem(Item item);

    public void UpdateItem(Item item);

    public void DeleteItem(Guid id);
}