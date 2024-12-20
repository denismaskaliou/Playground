using Azure.Messaging.ServiceBus;
using CosmosDb.Shared.Repository;
using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        var auditLogDto = message.Body.ToObjectFromJson<AuditLogDto>();
        if (auditLogDto is null)
        {
            logger.LogError("Can't parse audit log message");
            return;
        }

        var order = new AuditLog
        {
            EntityId = auditLogDto.EntityId,
            EventName = auditLogDto.EventName,
            CreatedDate = auditLogDto.CreatedDate
        };

        await auditLogsRepository.CreateAsync(order);
    }
}