namespace ServiceBus.Shared.Options;

public class ServiceBusBaseOptions
{
    public string ConnectionString { get; init; } = default!;
    public string QueueName { get; init; } = default!;
}