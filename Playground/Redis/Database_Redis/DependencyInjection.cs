using Database_Redis.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Database_Redis;

public static class DependencyInjection
{
    public static void AddRedisDependencies(this IServiceCollection services)
    {
        services.AddScoped<ProductsCachedService>();
    }
}