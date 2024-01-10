using Bookshop.Application.Features.Dto;
using Bookshop.Application.Features.ShoppingCarts;

namespace Bookshop.Application.Features.Orders
{
    public class OrderResponseDto : IBaseDto
    {
        public long Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VatRate { get; set; }
        public string? StatusOrder { get; set; }
        public string? DateOrder { get; set; }
        public string? MethodOfPayment { get; set; }
        public IList<ShopItemResponseDto>? Items { get; set; }
    }
}
