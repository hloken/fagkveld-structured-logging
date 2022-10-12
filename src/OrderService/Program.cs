using Microsoft.AspNetCore.Mvc;
using OrderService.Contracts;
using OrderService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
var logger = builder.AddLogging("orders");

// Configure Inventory client
var inventoryUri = builder.ConfigureInventoryHttpClient(logger);

// Build the app
var app = builder.Build();
app.UseHttpsRedirection();

// Place Order
app.MapPost("/place-order", async ([FromBody] PlaceOrderRequest request, InventoryClient inventoryService,
    HttpContext ctx, ILogger<Program> logger) =>
{
    var orderId = Guid.NewGuid();
    using var scope = logger.BeginScope(new Dictionary<string, object> { ["OrderId"] = orderId});

    // Validation
    if (string.IsNullOrWhiteSpace(request.ItemName))
    {
        logger.LogError("Order placed with empty item-name: {RequestContent}", request);
        return Results.BadRequest(new PlaceOrderResponse(false, Guid.Empty, "Order placed with empty item-name"));
    }

    logger.LogInformation("Received new order {OrderId} for item: {ItemName} quantity: {NumberOfItems}",
        orderId, request.ItemName, request.NumberOfItems);
    
    // Checking with inventory
    var success = await inventoryService.ReserveItems(request.ItemName, request.NumberOfItems, orderId, ctx.RequestAborted);
    if (!success)
    {
        logger.LogInformation(
            "Cannot place order, inventory-service could not reserve items for {OrderId}, item: {ItemName} Quantity: {NumberOfItems}",
            orderId, request.ItemName, request.NumberOfItems);
        return Results.BadRequest(new PlaceOrderResponse(false, orderId, "Inventory says no"));
    }

    logger.LogInformation("Successfully placed order {OrderId} for item: {ItemName}, quantity: {NumberOfItems}",
        orderId, request.ItemName, request.NumberOfItems);

    return Results.Accepted(value: new PlaceOrderResponse(true, orderId, string.Empty));
});

await app.RunAsync();
