using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Functions.App;

public class Function(ILogger<Function> logger)
{
    [Function("Playground")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks/get-text")]
        HttpRequestData request)
    {
        logger.LogInformation($"Playground function executed");

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("Text from body: Hello World!");

        return response;
    }
}