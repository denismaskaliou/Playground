using Database_Redis;
using StackExchange.Redis;
using WebAPI_WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { { "redis-19021.c300.eu-central-1-1.ec2.redns.redis-cloud.com", 19021 } },
        User = "default",
        Password = "Ul5syO0iYAJIh1YRhjUrswq5XJEPp0B3"
    };

});
builder.Services.AddRedisDependencies();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();