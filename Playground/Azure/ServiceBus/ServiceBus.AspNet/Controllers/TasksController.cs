using Microsoft.AspNetCore.Mvc;
using ServiceBus.AspNet.Models;
using ServiceBus.Shared.Producer;

namespace ServiceBus.AspNet.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("send-message")]
    public async Task<ActionResult> SendMessageAsync(ServiceBusMessageDto dto)
    {
        await messageProducer.SendMessageAsync(dto);
        return Accepted();
    }
}