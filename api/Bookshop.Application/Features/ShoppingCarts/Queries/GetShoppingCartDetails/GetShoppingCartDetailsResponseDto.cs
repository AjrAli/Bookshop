

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetailsResponseDto : ShoppingCartResponseDto
    {
        public long Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VatRate { get; set; }
    }
}
