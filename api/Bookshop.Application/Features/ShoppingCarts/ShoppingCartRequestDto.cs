using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartRequestDto : IBaseDto
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public IList<ShopItemRequestDto>? Items { get; set; }
    }
}
