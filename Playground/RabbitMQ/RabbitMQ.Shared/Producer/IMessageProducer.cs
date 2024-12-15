namespace RabbitMQ.Shared.Producer;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message);
}