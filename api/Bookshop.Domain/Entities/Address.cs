using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class Address : AuditableEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        private Address() { }
        public void EditAdress(Address address)
        {
            Street = address.Street;
            City = address.City;
            PostalCode = address.PostalCode;
            State = address.State;
            Country = address.Country;
        }
        public Address(string street, string city, string postalCode, string country, string state)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            State = state;
            Country = country;
        }
        // Relationships
        public LocationPricing? LocationPricing { get; set; }
        public long? LocationPricingId { get; set; }
    }
}
