
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
RegisterDependencies(builder.Services);

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    // services.AddHostedService<ServiceBusHostedService>();
    //
    // services.AddSingleton<IServiceBusConsumer>((s) =>
    // {
    //     var options = s.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
    //     return new ServiceBusConsumer(options);
    // });
    //     
    // services.AddTransient<IMessageProducer>(sp =>
    // {
    //     var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
    //     return new ServiceBusMessageProducer(options);
    // });
    //
    // services
    //     .AddOptions<ServiceBusOptions>()
    //     .Bind(builder.Configuration.GetSection(ServiceBusOptions.SectionName));
}