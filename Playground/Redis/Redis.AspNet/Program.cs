var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
RegisterDependencies(builder.Services);

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();


void RegisterDependencies(IServiceCollection services)
{
    
}