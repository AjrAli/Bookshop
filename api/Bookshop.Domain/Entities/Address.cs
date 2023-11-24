using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class Address : AuditableEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        private Address() { }
        public Address(string street, string city, string postalCode, string country, string state)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
            State = state;
        }
    }
}
