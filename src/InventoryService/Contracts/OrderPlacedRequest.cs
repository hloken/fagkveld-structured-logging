namespace InventoryService.Contracts;

public record struct OrderPlacedRequest(
    Guid OrderId, 
    string ItemName, 
    int NumberOfItems);