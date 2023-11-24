using Bookshop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public decimal SalesTax { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }
        [EnumDataType(typeof(CreditCards))]
        public CreditCards MethodOfPayment { get; set; }
        public DateTime DateOrder { get; set; }
        [EnumDataType(typeof(Status))]
        public Status StatusOrder { get; set; }
        
        private Order() { }

        public Order(decimal salesTax,
                     decimal shippingFee,
                     decimal total,
                     CreditCards creditCard,
                     DateTime dateOrder,
                     Status statusOrder,
                     long customerId)
        {
            SalesTax = salesTax;
            ShippingFee = shippingFee;
            Total = total;
            MethodOfPayment = creditCard;
            DateOrder = dateOrder;
            StatusOrder = statusOrder;
            CustomerId = customerId;
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
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<BookOrder> BookOrders { get; set; } = new List<BookOrder>();
    }

}
