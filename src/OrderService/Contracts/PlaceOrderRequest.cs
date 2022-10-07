namespace OrderService.Contracts;

public record struct PlaceOrderRequest(
    string ItemName,
    int NumberOfItems);