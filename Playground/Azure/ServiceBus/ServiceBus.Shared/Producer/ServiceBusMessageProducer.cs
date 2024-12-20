using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBus.Shared.Options;

namespace ServiceBus.Shared.Producer;

public class ServiceBusMessageProducer(ServiceBusBaseOptions options) : IMessageProducer
{
    private readonly ServiceBusClient _client = new(options.ConnectionString);

    public async Task SendMessageAsync<T>(T message)
    {
        await using var sender = _client.CreateSender(options.QueueName ?? options.TopicName);
        
        var body = JsonSerializer.Serialize(message);
        var busMessage = new ServiceBusMessage(body);
        
        await sender.SendMessageAsync(busMessage);
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}