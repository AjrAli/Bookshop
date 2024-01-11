
namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShopItemRequestDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
    }
}
