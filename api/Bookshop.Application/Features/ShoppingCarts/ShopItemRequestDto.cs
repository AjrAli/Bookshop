using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShopItemRequestDto : IBaseDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
    }
}
