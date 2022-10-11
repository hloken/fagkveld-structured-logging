using InventoryService;
using InventoryService.Contracts;
using InventoryService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var inventoryRepository = new InventoryRepository
{
    Items = new()
    {
        { "ice-cream", 3 },
        { "banana", 1 }
    }
};

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.AddLogging("inventory");

// Add inventory
builder.Services.AddSingleton<InventoryRepository>(inventoryRepository);

// Build the app
var app = builder.Build();
app.UseHttpsRedirection();

app.MapPost("/reserve-items", ([FromBody] ReserveItemsRequest request, InventoryRepository inventory) =>
    inventory.ReserveItems(request.ItemName, request.NumberOfItems) switch
    {
        true => Results.Ok(new ReserveItemsResponse { Success = true }),
        _ => Results.BadRequest(new ReserveItemsResponse { Success = false, ErrorMessage = "Out of stock" }),
    }
);

app.MapGet("", (InventoryRepository inventory) => 
    new GetAllResponse(
        inventory.Items.Select(item => new GetAllResponseItem(item.Key, item.Value)).ToArray())
);

await app.RunAsync();
