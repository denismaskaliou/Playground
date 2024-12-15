using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.Options;

namespace RabbitMQ.Shared.Consumer;

public class RabbitMqConsumer(RabbitMqBaseOptions options): IAsyncDisposable
{
    private IChannel? _channel;
    private IConnection? _connection;

    public async Task ConsumeAsync<T>(Func<T?, Task> onMessageReceived, CancellationToken token = default)
    {
        await InitAsync(token);

        await _channel!.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: token
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, message) =>
        {
            var body = message.Body.ToArray();
            var obj = JsonSerializer.Deserialize<T>(body);

            await onMessageReceived(obj);
        };

        await _channel.BasicConsumeAsync(
            queue: options.QueueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: token
        );
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }

    private async Task InitAsync(CancellationToken token = default)
    {
        if (_connection != null && _channel != null) return;
        
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password
        };

        _connection = await factory.CreateConnectionAsync(token);
        _channel = await _connection.CreateChannelAsync(cancellationToken: token);
    }
}
