using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBus.Shared.Options;

namespace ServiceBus.Shared.Consumer;

public class ServiceBusConsumer(ServiceBusBaseOptions options): IServiceBusConsumer
{
    private ServiceBusProcessor? _processor;
    private readonly ServiceBusClient _client = new(options.ConnectionString);

    public async Task ConsumeAsync<T>(Func<T?, Task> onMessageReceived, CancellationToken token = default)
    {
        _processor = _client.CreateProcessor(options.QueueName, new ServiceBusProcessorOptions());

        _processor.ProcessErrorAsync += ErrorHandler;
        _processor.ProcessMessageAsync += async args =>
        {
            var body = JsonSerializer.Deserialize<T>(args.Message.Body);
            await onMessageReceived(body);
        };

        await _processor.StartProcessingAsync(token);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }


    public async ValueTask DisposeAsync()
    {
        if (_processor != null) await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }
}