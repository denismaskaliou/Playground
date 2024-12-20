using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServiceBus.Shared.Producer;

namespace Functions.App.Functions;

public class OnSubmitOrderFunction(
    IMessageProducer messageProducer,
    ILogger<SubmitOrderFunction> logger)
{
    [Function("OnSubmittedOrder")]
    public async Task RunAsync(
        [CosmosDBTrigger(
            databaseName: "playground",
            containerName: "orders",
            Connection = "CosmosDb:ConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true
        )]
        IReadOnlyList<Order> orders)
    {
        logger.LogInformation("[OnSubmittedOrder]: Run function.");

        foreach (var order in orders)
        {
            var eventLogDto = new OrderSubmittedMessage
            {
                OrderId = order.Id,
                EventName = "OrderSubmitted",
                CreatedDate = DateTime.UtcNow
            };
            
            await messageProducer.SendMessageAsync(eventLogDto);
        }
    }
}