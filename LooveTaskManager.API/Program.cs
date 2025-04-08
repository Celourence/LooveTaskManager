using Serilog;
using LooveTaskManager.API.Configuration;

namespace LooveTaskManager.API;

public class Program
{
    public static void Main(string[] args)
    {
        CultureConfiguration.ConfigureCulture();
        LoggingConfiguration.ConfigureLogging();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        startup.Configure(app, app.Environment);

        app.Run();
    }
}