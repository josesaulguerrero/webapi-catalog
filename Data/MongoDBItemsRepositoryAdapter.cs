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

    public async Task DeleteItemAsync(Guid id)
    {
        await _items.DeleteOneAsync(item => item.Id == id);
    }

    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        var items = await _items.FindAsync(new BsonDocument());
        return await items.ToListAsync();
    }

    public async Task<Item> GetItemByIdAsync(Guid id)
    {
        var item = await _items.FindAsync(_filterDefinitionBuilder.Eq(item => item.Id, id));
        return await item.SingleOrDefaultAsync();
    }

    public async Task SaveItemAsync(Item item)
    {
        await _items.InsertOneAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        await _items.ReplaceOneAsync(_filterDefinitionBuilder.Eq(item => item.Id, item.Id), item);
    }
}