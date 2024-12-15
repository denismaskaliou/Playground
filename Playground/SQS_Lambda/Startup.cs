using Amazon.Lambda.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace SQS_Lambda;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAWSMessageBus(builder =>
        {
            // Register that you'll publish messages of type "GreetingMessage" to the specified queue URL.
            //  1. When deployed, the QUEUE_URL variable will be set to the queue that is defined in serverless.template
            //  2. When testing locally using the Mock Lambda Test Tool, the queue URL is configured in launchSettings.json
            builder.AddSQSPublisher<GreetingMessage>(Environment.GetEnvironmentVariable("QUEUE_URL"));

            // You can register additional message types and destinations here as well.

            // Register that you'll process messages in a Lambda function, and that messages
            // of the GreetingMessage type will be processed by GreetingMessageHandler
            builder.AddLambdaMessageProcessor();
            builder.AddMessageHandler<GreetingMessageHandler, GreetingMessage>();

            // You can register additional message type and handler mappings here as well.
        });
    }
}

