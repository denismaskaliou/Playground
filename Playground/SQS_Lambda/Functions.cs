using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.Messaging;
using AWS.Messaging.Lambda;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SQS_Lambda;

public class Functions
{
    private ILambdaMessaging _lambdaMessaging;

    public Functions(ILambdaMessaging lambdaMessaging)
    {
        _lambdaMessaging = lambdaMessaging;
    }

    public async Task Sender([FromServices] IMessagePublisher publisher, GreetingMessage message, ILambdaContext context)
    {
        if (message == null)
        {
            return;
        }

        context.Logger.LogInformation($"Received '{message.Greeting}' from '{message.SenderName}', will send to SQS");

        await publisher.PublishAsync(message);
    }

    [LambdaFunction(Policies = "AWSLambdaSQSQueueExecutionRole")]
    [SQSEvent("@MessageProcessingFrameworkDemoQueue", ResourceName = "SQSEvent")]
    public async Task<SQSBatchResponse> Handler(SQSEvent evnt, ILambdaContext context)
    {
        return await _lambdaMessaging.ProcessLambdaEventWithBatchResponseAsync(evnt, context);
    }
}