using System.Linq.Expressions;
using CosmosDb.Shared.Entities;
using CosmosDb.Shared.Options;
using Microsoft.Azure.Cosmos;

namespace CosmosDb.Shared.Repository;

public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : BaseEntity
{
    private readonly Container _container;

    public CosmosDbRepository(CosmosClient client, CosmosDbBaseOptions options, string containerName)
    {
        _container = client.GetContainer(options.DatabaseName, containerName);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return null!;
    }

    public async Task<T> GetByIdAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null!;
        }
    }

    public async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> filter)
    {
        return null!;
    }

    public async Task<T> CreateAsync(T entity)
    {
        entity.Id ??= Guid.NewGuid().ToString();
        await _container.CreateItemAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _container.UpsertItemAsync(entity, new PartitionKey(id));
    }

    public async Task DeleteAsync(string id)
    {
        await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
    }
}