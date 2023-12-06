using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartDto : IBaseDto
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public IList<ShopItemDto>? Items { get; set; }
    }
}
