using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetailsResponseDto : IBaseDto
    {
        public long Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VatRate { get; set; }
        public IList<ShopItemResponseDto>? Items { get; set; }
    }
}
