namespace InventoryService.Contracts;

public record OrderPlacedRequest(
    Guid OrderId, 
    string ItemName, 
    int NumberOfItems);
    
public record OrderPlacedResponse(
    bool Success,
    string? ErrorMessage);