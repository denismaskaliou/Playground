using Microsoft.Extensions.Options;
using RabbitMQ.AspNet.Models;
using RabbitMQ.AspNet.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.Consumer;

namespace RabbitMQ.AspNet.HostedServices;

public class RabbitMqHostedService(
    IServiceProvider serviceProvider,
    RabbitMqConsumer rabbitMqConsumer
) : BackgroundService
{
    private bool ConsumerIsActive => serviceProvider
        .GetRequiredService<IOptions<RabbitMqOptions>>().Value
        .ConsumerIsActive;
    
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        if (!ConsumerIsActive) return;

        await rabbitMqConsumer.ConsumeAsync<RabbitMqMessageDto>(ConsumeMessageAsync, token);
    }

    private async Task ConsumeMessageAsync(RabbitMqMessageDto? message)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RabbitMqHostedService>>();

        logger.LogInformation("Consume message: {Message}", message?.Message);
    }

}