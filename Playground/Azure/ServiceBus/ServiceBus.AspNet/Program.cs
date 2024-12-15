using Microsoft.Extensions.Options;
using ServiceBus.AspNet.HostedServices;
using ServiceBus.AspNet.Options;
using ServiceBus.Shared.Consumer;
using ServiceBus.Shared.Producer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Configuration.AddUserSecrets<Program>();
RegisterDependencies(builder.Services);

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    services.AddHostedService<ServiceBusHostedService>();

    services.AddSingleton<IServiceBusConsumer>((s) =>
    {
        var options = s.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
        return new ServiceBusConsumer(options);
    });
        
    services.AddTransient<IMessageProducer>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
        return new ServiceBusMessageProducer(options);
    });

    services
        .AddOptions<ServiceBusOptions>()
        .Bind(builder.Configuration.GetSection(ServiceBusOptions.SectionName));
}