using Bookshop.Application.Features.Dto;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Features.Customers
{
    public class CustomerDto : IBaseDto
    {
        public long Id { get ; set ; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ShippingAddressId { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public long? BillingAddressId { get; set; }
        public AddressDto? BillingAddress { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? IdentityUserDataId { get; set; }
    }
}
