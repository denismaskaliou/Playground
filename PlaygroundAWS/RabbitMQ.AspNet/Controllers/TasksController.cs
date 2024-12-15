using Microsoft.AspNetCore.Mvc;
using RabbitMQ.AspNet.Models;
using RabbitMQ.Shared.Producer;

namespace RabbitMQ.AspNet.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(
    IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("send-message")]
    public async Task<ActionResult> SendMessageAsync(RabbitMqMessageDto dto)
    {
        await messageProducer.SendMessageAsync(dto);
        return Accepted();
    }
}