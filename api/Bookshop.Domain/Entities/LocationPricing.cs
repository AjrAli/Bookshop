using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class LocationPricing : AuditableEntity
    {
        public string Country { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VatRate { get; set; }
        private LocationPricing() { }
        public LocationPricing(string country, decimal shippingFee, decimal vatRate)
        {
            Country = country;
            ShippingFee = shippingFee;
            VatRate = vatRate;
        }
    }
}
