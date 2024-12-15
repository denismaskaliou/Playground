using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Shared.Entities;

namespace MongoDB.Shared.Repository;

public class MongoDbRepository<T> : IMongoDbRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public MongoDbRepository(MongoDbContext context, string collectionName)
    {
        _collection = context.Database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(e => e.Id == id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(e => e.Id == id);
    }
}