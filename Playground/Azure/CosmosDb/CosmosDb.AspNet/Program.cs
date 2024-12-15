using CosmosDb.AspNet.Models;
using CosmosDb.AspNet.Options;
using CosmosDb.Shared.Entities;
using CosmosDb.Shared.Repository;
using CosmosDb.Shared.Serializer;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Configuration.AddUserSecrets<Program>();
RegisterDependencies(builder.Services);

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    services.AddScoped<ICosmosDbRepository<Product>>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

        var clientOptions = new CosmosClientOptions { Serializer = new CustomCosmosSerializer() };
        var client = new CosmosClient(options.ConnectionString, clientOptions);

        return new CosmosDbRepository<Product>(client, options, "products");
    });

    services
        .AddOptions<CosmosDbOptions>()
        .Bind(builder.Configuration.GetSection(CosmosDbOptions.SectionName));
}