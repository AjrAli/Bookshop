using Bookshop.Application.Features.ShoppingCarts;

namespace Bookshop.Application.Features.Customers
{
    public class CustomerResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public ShoppingCartResponseDto? ShoppingCart {  get; set; }
    }
}
