namespace ServiceBus.Shared.Producer;

public interface IMessageProducer: IAsyncDisposable
{
    Task SendMessageAsync<T>(T message);
}