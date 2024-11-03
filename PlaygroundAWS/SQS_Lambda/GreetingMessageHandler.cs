using Amazon.Lambda.Core;
using AWS.Messaging;

namespace SQS_Lambda;

public class GreetingMessageHandler : IMessageHandler<GreetingMessage>
{
    public ILambdaContext _context;

    public GreetingMessageHandler(ILambdaContext context)
    {
        _context = context;
    }

    public Task<MessageProcessStatus> HandleAsync(MessageEnvelope<GreetingMessage> messageEnvelope, CancellationToken token = default)
    {
        // The outer envelope contains metadata, and its Message property contains the actual message content
        var greetingMessage = messageEnvelope.Message;

        if (string.IsNullOrEmpty(greetingMessage.Greeting) || string.IsNullOrEmpty(greetingMessage.SenderName))
        {
            _context.Logger.LogError($"Received a message that was missing the {nameof(GreetingMessage.Greeting)} " +
                $"and/or the {nameof(GreetingMessage.SenderName)} from message {messageEnvelope.Id}");

            return Task.FromResult(MessageProcessStatus.Failed());
        }

        _context.Logger.LogInformation($"Received '{greetingMessage.Greeting}' from '{greetingMessage.SenderName}'");

        return Task.FromResult(MessageProcessStatus.Success());
    }
}