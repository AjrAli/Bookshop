using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Customers
{
    public class CustomerResponseDto : IBaseDto
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ShippingAddressId { get; set; }
        public long? BillingAddressId { get; set; }
        public string? IdentityUserDataId { get; set; }
    }
}
