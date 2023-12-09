namespace Bookshop.Application.Features.Customers
{
    public class CustomerResponseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ShippingAddressId { get; set; }
        public long? BillingAddressId { get; set; }
    }
}
