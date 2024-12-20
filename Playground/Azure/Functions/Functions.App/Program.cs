using Blob.Shared.Storages;
using CosmosDb.Shared.Extensions;
using Functions.App.Entities;
using Functions.App.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServiceBus.Shared.Producer;

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
        // Repositories
        services.AddCosmosDbRepository<Order, CosmosDbOptions>("orders");
        services.AddCosmosDbRepository<AuditLog, CosmosDbOptions>("audit-logs");
        
        services
            .AddOptions<CosmosDbOptions>()
            .Bind(host.Configuration.GetSection(CosmosDbOptions.SectionName));

        // Message producer
        services.AddTransient<IMessageProducer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
            options.TopicName = options.OrderCreatedTopicName;

            return new ServiceBusMessageProducer(options);
        });
        
        services
            .AddOptions<ServiceBusOptions>()
            .Bind(host.Configuration.GetSection(ServiceBusOptions.SectionName));

        // BlobStorage
        services.AddTransient<IBlobStorage>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<BlobOptions>>().Value;
            options.ContainerName = options.OrdersStorageName;

            return new BlobStorage(options);
        });
        
        services
            .AddOptions<BlobOptions>()
            .Bind(host.Configuration.GetSection(BlobOptions.SectionName));
    })
    .Build();

host.Run();