namespace OrderService.Contracts;

public record struct PlaceOrderResponse(
    bool Success,
    Guid OrderId,
    string ErrorMessage
);