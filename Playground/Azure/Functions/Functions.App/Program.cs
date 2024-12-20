using CosmosDb.Shared.Repository;
using CosmosDb.Shared.Serializer;
using Functions.App.Models;
using Functions.App.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((builder, config) =>
    {
        config.AddUserSecrets<Program>();
        var settings = config.Build();
        var connectionString = settings["AppConfigConnectionString"];
        config.AddAzureAppConfiguration(connectionString);
    })
    .ConfigureServices((host, services) =>
    {
        services.AddScoped<ICosmosDbRepository<Order>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
            var clientOptions = new CosmosClientOptions { Serializer = new CustomCosmosSerializer() };
            var client = new CosmosClient(options.ConnectionString, clientOptions);

            return new CosmosDbRepository<Order>(client, options, "orders");
        });

        services
            .AddOptions<CosmosDbOptions>()
            .Bind(host.Configuration.GetSection(CosmosDbOptions.SectionName));
    })
    .Build();

host.Run();