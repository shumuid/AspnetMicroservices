namespace Basket.API.Entities
{
    public record ShoppingCartItem
    {
        public int Quantity { get; init; }
        public string Color { get; init; }
        public decimal Price { get; init; }
        public string ProductId { get; init; }
        public string ProductName { get; init; }
    }
}