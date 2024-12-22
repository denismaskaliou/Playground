using System.Net;
using CosmosDb.Shared.Repository;
using Functions.App.Entities;
using Functions.App.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Functions.App.Functions;

public class SubmitOrderFunction(
    ILogger<SubmitOrderFunction> logger,
    ICosmosDbRepository<Order> ordersRepository)
{
    [Function("SubmitOrder")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks/submit-orders")]
        HttpRequestData request)
    {
        logger.LogInformation("Submit order request");

        var order = new Order
        {
            Name = "Test1",
            OrderStatus = OrderStatus.Submitted,
            UpdatedAt = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };

        await ordersRepository.CreateAsync(order);

        return request.CreateResponse(HttpStatusCode.Accepted);
    }
}