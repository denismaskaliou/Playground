using CosmosDb.Shared.Entities;
using CosmosDb.Shared.Options;
using CosmosDb.Shared.Repository;
using CosmosDb.Shared.Serializer;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CosmosDb.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosDbRepository<TEntity, TOptions>(
        this IServiceCollection services, 
        string containerName)
        where TEntity : BaseEntity
        where TOptions : CosmosDbBaseOptions
    {
        services.AddScoped<ICosmosDbRepository<TEntity>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TOptions>>().Value;
            var clientOptions = new CosmosClientOptions { Serializer = new CustomCosmosSerializer() };
            var client = new CosmosClient(options.ConnectionString, clientOptions);

            return new CosmosDbRepository<TEntity>(client, options, containerName);
        });
        
        return services;
    }
}