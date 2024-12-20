using ServiceBus.Shared.Options;

namespace Functions.App.Options;

public class ServiceBusOptions : ServiceBusBaseOptions
{
    public const string SectionName = "ServiceBus";

    public string OrderCreatedTopicName { get; set; } = default!;
}