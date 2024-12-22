using Azure.Messaging.ServiceBus;
using CosmosDb.Shared.Repository;
using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.App.Functions;

public class AuditOrdersFunction(
    ILogger<AuditOrdersFunction> logger,
    ICosmosDbRepository<AuditLog> auditLogsRepository)
{
    [Function("AuditOrders")]
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
        logger.LogInformation("Log audit");

        foreach (var order in orders)
        {
            var auditLog = new AuditLog
            {
                EntityId = order.Id,
                EntityName = nameof(Order),
                EventName = AuditType.Created,
                Changes = new List<AuditChange>(),
                ModifiedBy = order.UpdatedBy,
                ModifiedAt = order.UpdatedAt
            };

            await auditLogsRepository.CreateAsync(auditLog);
        }
    }
}