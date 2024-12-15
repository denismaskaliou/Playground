using Microsoft.Extensions.Options;
using RabbitMQ.AspNet.HostedServices;
using RabbitMQ.AspNet.Options;
using RabbitMQ.Shared.Consumer;
using RabbitMQ.Shared.Producer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
RegisterDependencies(builder.Services);
    
var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    services.AddHostedService<RabbitMqHostedService>();

    services.AddScoped<IMessageProducer>((s) =>
    {
        var options = s.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        return new RabbitMqProducer(options);
    });
    
    services.AddSingleton<RabbitMqConsumer>((s) =>
    {
        var options = s.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        return new RabbitMqConsumer(options);
    });

    services
        .AddOptions<RabbitMqOptions>()
        .Bind(builder.Configuration.GetSection(RabbitMqOptions.SectionName));
}