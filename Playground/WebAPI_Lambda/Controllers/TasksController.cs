using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Lambda.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    [HttpPost("send-sqs-message")]
    public IActionResult SendSqsMessage()
    {
        return Ok();
    }
}