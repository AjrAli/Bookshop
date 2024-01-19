namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartResponseDto
    {
        public long Id {  get; set; } 
        public decimal Total { get; set; }
        public IList<ShopItemResponseDto>? Items { get; set; }
    }
}
