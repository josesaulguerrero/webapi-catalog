using Catalog.Domain;
using MongoDB.Driver;

namespace Catalog.Data;

public class MongoDBItemsRepositoryAdapter : ItemsRepositoryContract
{
    private const string _DBName = "Catalog";
    private const string _DBCollectionName = "items";
    private readonly IMongoCollection<Item> _items;

    public MongoDBItemsRepositoryAdapter(IMongoClient mongoClient)
    {
        IMongoDatabase db = mongoClient.GetDatabase(_DBName);
        _items = db.GetCollection<Item>(_DBCollectionName);
    }

    public void DeleteItem(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Item> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public Item GetItemById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Item SaveItem(Item item)
    {
        _items.InsertOne(item);
    }

    public void UpdateItem(Item item)
    {
        throw new NotImplementedException();
    }
}