using Azure.Messaging.ServiceBus;
using Blob.Shared.Storages;
using CosmosDb.Shared.Repository;
using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions.App.Functions;

public class StoreOrderFunction(
    IBlobStorage blobStorage,
    ILogger<StoreOrderFunction> logger,
    ICosmosDbRepository<Order> ordersRepository)
{
    [Function("StoreOrder")]
    public async Task RunAsync(
        [ServiceBusTrigger("order-created", "store-order-subscription", Connection = "ServiceBus:ConnectionString")]
        ServiceBusReceivedMessage message)
    {
        logger.LogInformation("Log event");

        var submittedMessage = message.Body.ToObjectFromJson<OrderSubmittedMessage>();
        if (submittedMessage is null)
        {
            logger.LogError("Can't parse submitted message");
            return;
        }

        var order = await ordersRepository.GetByIdAsync(submittedMessage.OrderId);
        if (order is null)
        {
            throw new Exception($"Order with id {submittedMessage.OrderId} was not found");
        }

        var json = JsonConvert.SerializeObject(order);
        var fileName = $"{submittedMessage.OrderId}.json";

        await blobStorage.UploadAsync(fileName, json);
    }
}