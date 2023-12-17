using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomerDto
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
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
