using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Shared.Options;

namespace RabbitMQ.Shared.Producer;

public class RabbitMqProducer(RabbitMqBaseOptions options) : IMessageProducer
{
    public async Task SendMessageAsync<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: options.QueueName,
            body: body
        );
    }
}