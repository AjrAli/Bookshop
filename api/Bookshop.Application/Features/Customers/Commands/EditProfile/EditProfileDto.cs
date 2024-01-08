using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Customers.Commands.EditProfile
{
    public class EditProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ShippingAddressId { get; set; }
        public AddressDto? ShippingAddress { get; set; }
        public long? BillingAddressId { get; set; }
        public AddressDto? BillingAddress { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
