using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace InventoryService.Infrastructure;

public static class Logging
{
    public static Logger CreateLogger(IConfiguration configuration, string applicationName)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.WithEnvironmentName()
            .Enrich.FromLogContext()
            .CreateLogger();
    }
}