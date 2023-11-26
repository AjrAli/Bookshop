using Bookshop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public decimal SalesTax { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; private set; }
        [EnumDataType(typeof(CreditCards))]
        public CreditCards MethodOfPayment { get; set; }
        public DateTime DateOrder { get;}
        [EnumDataType(typeof(Status))]
        public Status StatusOrder { get; set; }
        
        private Order() { }

        public Order(decimal salesTax,
                     decimal shippingFee,
                     CreditCards creditCard,
                     Status statusOrder)
        {
            SalesTax = salesTax;
            ShippingFee = shippingFee;
            MethodOfPayment = creditCard;
            StatusOrder = statusOrder;
            DateOrder = DateTime.Now;
        }

        public enum CreditCards
        {
            AmericanExpress,
            Discover,
            Mastercard,
            Visa
        }

        public enum Status
        {
            Cancelled,
            Pending,
            AwaitingPayment,
            AwaitingShipment,
            Completed
        }

        // Relationships
        public long? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
        public void CalculateTotal()
        {
            Total = 0;
            if (LineItems != null && LineItems.Count > 0)
            {
                var totalWithoutTaxes = LineItems.Sum(x => x.Price);
                Total = totalWithoutTaxes + ((totalWithoutTaxes / 100) * SalesTax) + ShippingFee;
            }
        }
    }

}
