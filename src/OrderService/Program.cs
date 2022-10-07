using Microsoft.AspNetCore.Mvc;
using OrderService;
using OrderService.Contracts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Logging.ClearProviders(); // remove default logging providers

using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("ApplicationName", "Orders")
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Configure Inventory client
var defaultInventoryUri = "https://localhost:5003";
var inventoryUri = builder.Configuration.GetServiceUri("inventory", "https")
                     ?? new Uri(defaultInventoryUri);
builder.Services.AddHttpClient<InventoryClient>(client =>
{
    client.BaseAddress = inventoryUri;
});
logger.Information("{ServiceClientName} is configured at URL {ServiceURL}", nameof(InventoryClient), inventoryUri);

// Build the app
var app = builder.Build();
app.UseHttpsRedirection();

// The routes
app.MapPost("/place-order", async ([FromBody] PlaceOrderRequest request, ILogger<Program> logger) =>
{
    // Validation
    if (string.IsNullOrWhiteSpace(request.ItemName))
    {
        logger.LogError("Order placed with empty item-name: {Request}", request);

        return new PlaceOrderResponse(false, Guid.Empty, "Order placed with empty item-name");
    }

    // Notifying inventory
    // ...and handling problems

    return new PlaceOrderResponse(true, Guid.NewGuid(), "Order placed successfully");
})
.WithName("PlaceOrder");



await app.RunAsync();
