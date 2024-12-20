using CosmosDb.Shared.Extensions;
using Functions.App.Entities;
using Functions.App.Models;
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
        
        // Message producer
        services.AddTransient<IMessageProducer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
            options.TopicName = options.OrderCreatedTopicName;
            
            return new ServiceBusMessageProducer(options);
        });
        
        // Options
        services
            .AddOptions<CosmosDbOptions>()
            .Bind(host.Configuration.GetSection(CosmosDbOptions.SectionName));
        
        services
            .AddOptions<ServiceBusOptions>()
            .Bind(host.Configuration.GetSection(ServiceBusOptions.SectionName));
    })
    .Build();

host.Run();