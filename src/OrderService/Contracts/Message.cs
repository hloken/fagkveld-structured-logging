public record PlaceOrderRequest(
    string ItemName,
    int NumberOfItems);

public record PlaceOrderResponse(
    bool Success,
    Guid OrderId,
    string ErrorMessage
);
    