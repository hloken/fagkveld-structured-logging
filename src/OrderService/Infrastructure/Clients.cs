using Serilog.Core;

namespace OrderService.Infrastructure;

public static class Clients
{
    public static Uri ConfigureInventoryHttpClient(this WebApplicationBuilder builder, Logger logger)
    {
        var inventoryUri = builder.Configuration.GetServiceUri("inventory", "https")
                           ?? new Uri("https://localhost:5003");

        builder.Services.AddHttpClient<InventoryClient>(client =>
        {
            client.BaseAddress = inventoryUri;
        })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
            });

        logger.Information("{ServiceClientName} is configured at URL {ServiceUrl}", nameof(InventoryClient), inventoryUri);

        return inventoryUri;
    }
}