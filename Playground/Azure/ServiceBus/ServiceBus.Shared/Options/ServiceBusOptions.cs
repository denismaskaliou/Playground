namespace ServiceBus.Shared.Options;

public class ServiceBusBaseOptions
{
    public string ConnectionString { get; init; } = default!;
    public string? QueueName { get; set; }
    public string? TopicName { get; set; }
}