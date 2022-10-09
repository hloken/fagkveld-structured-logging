using Serilog.Core;
using Serilog.Events;

namespace OrderService.Infrastructure;

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

    public static Logger AddLogging(this WebApplicationBuilder builder, string applicationName)
    {
        builder.Logging.ClearProviders(); // remove default logging providers
        var logger = CreateLogger(builder.Configuration, applicationName);
        builder.Logging.AddSerilog(logger);

        return logger;
    }
}