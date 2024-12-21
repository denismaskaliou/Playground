using Azure.Messaging.ServiceBus;
using CosmosDb.Shared.Repository;
using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.App.Functions;

public class LogEventFunction(
    ILogger<LogEventFunction> logger,
    ICosmosDbRepository<AuditLog> auditLogsRepository)
{
    [Function("LogEvent")]
    public async Task RunAsync(
        [ServiceBusTrigger("log-event-queue", Connection = "ServiceBus:ConnectionString")]
        ServiceBusReceivedMessage message)
    {
        logger.LogInformation("Log event");

        var submittedMessage = message.Body.ToObjectFromJson<OrderSubmittedMessage>();
        if (submittedMessage is null)
        {
            logger.LogError("Can't parse submitted message");
            return;
        }

        var order = new AuditLog
        {
            EntityId = submittedMessage.OrderId,
            EventName = submittedMessage.EventName,
            CreatedDate = submittedMessage.CreatedDate
        };

        await auditLogsRepository.CreateAsync(order);
    }
}