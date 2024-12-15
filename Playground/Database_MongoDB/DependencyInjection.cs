using Database_MongoDB.Models;
using Database_MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Database_MongoDB;

public static class DependencyInjection
{
    public static void AddMongoDbDependencies(this IServiceCollection services)
    {
        services.AddScoped<ProductsRepository>();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }
}