using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBus.Shared.Options;

namespace ServiceBus.Shared.Consumer;

public interface IServiceBusConsumer: IAsyncDisposable
{
    public Task ConsumeAsync<T>(Func<T?, Task> onMessageReceived, CancellationToken token = default);
}