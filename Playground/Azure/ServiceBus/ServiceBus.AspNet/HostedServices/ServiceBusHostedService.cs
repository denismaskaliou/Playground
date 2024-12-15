using Microsoft.Extensions.Options;
using ServiceBus.AspNet.Models;
using ServiceBus.AspNet.Options;
using ServiceBus.Shared.Consumer;

namespace ServiceBus.AspNet.HostedServices;

public class ServiceBusHostedService(
    IServiceProvider serviceProvider,
    IServiceBusConsumer serviceBusConsumer
) : BackgroundService
{
    private bool ConsumerIsActive => serviceProvider
        .GetRequiredService<IOptions<ServiceBusOptions>>().Value
        .ConsumerIsActive;
    
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        if (!ConsumerIsActive) return;

        await serviceBusConsumer.ConsumeAsync<ServiceBusMessageDto>(ConsumeMessageAsync, token);
    }

    private async Task ConsumeMessageAsync(ServiceBusMessageDto? message)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ServiceBusHostedService>>();

        logger.LogInformation("Consume message: {Message}", message?.Message);
    }

}