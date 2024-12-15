using Microsoft.Extensions.Options;
using MongoDB.AspNet.Models;
using MongoDB.AspNet.Options;
using MongoDB.Shared;
using MongoDB.Shared.Entities;
using MongoDB.Shared.Mappings;
using MongoDB.Shared.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
RegisterDependencies(builder.Services);

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    services.AddTransient<MongoDbContext>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
        return new MongoDbContext(options);
    });
    
    services.AddScoped<IMongoDbRepository<Product>>(sp =>
    {
        ProductsMapping.Map();
        var context = sp.GetRequiredService<MongoDbContext>();
        return new MongoDbRepository<Product>(context, "products");
    });
    
    services
        .AddOptions<MongoDbOptions>()
        .Bind(builder.Configuration.GetSection(MongoDbOptions.SectionName));
}