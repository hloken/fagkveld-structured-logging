namespace InventoryService.Contracts;

public record struct OrderPlacedResponse(
    bool Success,
    string? ErrorMessage);