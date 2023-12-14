namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartRequestDto
    {
        public string? UserId { get; set; }
        public IList<ShopItemRequestDto>? Items { get; set; }
    }
}
