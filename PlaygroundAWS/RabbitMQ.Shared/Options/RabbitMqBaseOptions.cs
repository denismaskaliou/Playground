namespace RabbitMQ.Shared.Options;

public abstract class RabbitMqBaseOptions
{
    public string HostName { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string QueueName { get; init; } = null!;
}