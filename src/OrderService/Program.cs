using Microsoft.AspNetCore.Mvc;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

var defaultInventoryUri = "https://localhost:5003";

var inventoryUri = builder.Configuration.GetServiceUri("inventory", "https")
                     ?? new Uri(defaultInventoryUri);

builder.Services.AddHttpClient<InventoryClient>(client =>
{
    client.BaseAddress = inventoryUri;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Test comment
app.UseHttpsRedirection();

app.MapPost("/place-order", async ([FromBody]PlaceOrderRequest request, ILogger<Program> logger) =>
{
    // Validation
    if (string.IsNullOrWhiteSpace(request.ItemName))
    {
        logger.LogError("Order placed with empty item-name: {Request}", request);
        
        return  new PlaceOrderResponse(false, Guid.Empty, "Order placed with empty item-name");
    }

    // Notifying inventory
    // ...and handling problems
    
    return new PlaceOrderResponse(true, Guid.NewGuid(), "Order places successfully");
})
.WithName("PlaceOrder");

app.Run();
