using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.App;

public class Function(ILogger<Function> logger)
{
    [Function("Playground")]
    public async Task<ObjectResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "api/tasks/get-text")] HttpRequest request)
    {
        var text = await request.ReadFromJsonAsync<string>();
        logger.LogInformation($"Playground function read: {text}");
        
        return new OkObjectResult(text);
    }
}