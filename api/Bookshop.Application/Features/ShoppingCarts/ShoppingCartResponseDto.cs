using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.ShoppingCarts
{
    public class ShoppingCartResponseDto : IBaseDto
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public decimal Total { get; set; }
        public IList<ShopItemResponseDto>? Items { get; set; }
    }
}
