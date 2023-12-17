using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Customers
{
    public class CustomerRequestDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ShippingAddressId { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public long? BillingAddressId { get; set; }
        public AddressDto? BillingAddress { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
