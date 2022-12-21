using Catalog.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Data;

public class MongoDBItemsRepositoryAdapter : ItemsRepositoryContract
{
    private const string _DBName = "Catalog";
    private const string _DBCollectionName = "items";
    private readonly IMongoCollection<Item> _items;
    private static readonly FilterDefinitionBuilder<Item> _filterDefinitionBuilder = Builders<Item>.Filter;

    public MongoDBItemsRepositoryAdapter(IMongoClient mongoClient)
    {
        IMongoDatabase db = mongoClient.GetDatabase(_DBName);
        _items = db.GetCollection<Item>(_DBCollectionName);
    }

    public void DeleteItem(Guid id)
    {
        _items.DeleteOne(item => item.Id == id);
    }

    public IEnumerable<Item> GetAllItems()
    {
        return _items.Find(new BsonDocument()).ToList();
    }

    public Item GetItemById(Guid id)
    {
        return _items.Find(_filterDefinitionBuilder.Eq(item => item.Id, id)).SingleOrDefault();
    }

    public void SaveItem(Item item)
    {
        _items.InsertOne(item);
    }

    public void UpdateItem(Item item)
    {
        _items.ReplaceOne(_filterDefinitionBuilder.Eq(item => item.Id, item.Id), item);
    }
}