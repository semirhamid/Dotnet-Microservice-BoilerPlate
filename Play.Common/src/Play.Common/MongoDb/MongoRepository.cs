using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Catalog.Service.Repository;
using Play.Common.Entities;

namespace Play.Common.Repository
{
  public class MongoRepository<T> : IRepository<T> where T : IEntity
  {
    private readonly IMongoCollection<T> _itemsCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
    {
      _itemsCollection = mongoDatabase.GetCollection<T>(
            collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
      var items = await _itemsCollection.Find(filterBuilder.Empty).ToListAsync();
      return items;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
      return await _itemsCollection.Find(filter).ToListAsync();
    }

    public async Task<T> GetAsync(Guid id)
    {
      var filter = filterBuilder.Eq(item => item.Id, id);
      var item = await _itemsCollection.Find(filter).FirstOrDefaultAsync();
      return item;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
      return await _itemsCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T item)
    {
      await _itemsCollection.InsertOneAsync(item);
    }

    public async Task UpdateAsync(T item)
    {
      var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
      await _itemsCollection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(Guid id)
    {
      var filter = filterBuilder.Eq(item => item.Id, id);
      await _itemsCollection.DeleteOneAsync(filter);
    }

  }
}