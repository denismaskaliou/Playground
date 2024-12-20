using CosmosDb.Shared.Extensions;
using Functions.App.Models;
using Functions.App.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        services.AddCosmosDbRepository<Order, CosmosDbOptions>("orders");
        services
            .AddOptions<CosmosDbOptions>()
            .Bind(host.Configuration.GetSection(CosmosDbOptions.SectionName));
    })
    .Build();

host.Run();