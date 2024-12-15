using Microsoft.Extensions.Hosting;

namespace Functions.App;

public static class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        host.Run();
    }
}