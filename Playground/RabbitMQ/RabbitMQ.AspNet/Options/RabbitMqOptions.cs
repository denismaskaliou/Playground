using RabbitMQ.Shared.Options;

namespace RabbitMQ.AspNet.Options;

public class RabbitMqOptions : RabbitMqBaseOptions
{
    public const string SectionName = "RabbitMq";

    public bool ConsumerIsActive { get; init; }
}