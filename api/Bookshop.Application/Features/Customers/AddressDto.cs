﻿

namespace Bookshop.Application.Features.Customers
{
    public class AddressDto
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
