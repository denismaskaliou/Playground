using ServiceBus.Shared.Options;

namespace ServiceBus.AspNet.Options;

public class ServiceBusOptions : ServiceBusBaseOptions
{
    public const string SectionName = "ServiceBus";

    public bool ConsumerIsActive { get; init; }
}