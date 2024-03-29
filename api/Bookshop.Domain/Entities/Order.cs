﻿using Bookshop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        [EnumDataType(typeof(CreditCards))]
        public CreditCards MethodOfPayment { get; set; }
        public DateTime DateOrder { get; }
        [EnumDataType(typeof(Status))]
        public Status StatusOrder { get; set; }

        private Order() { }
        public Order(CreditCards creditCard, Customer customer, ICollection<LineItem> lineItems)
        {
            MethodOfPayment = creditCard;
            StatusOrder = Status.Pending;
            DateOrder = DateTime.Now;
            Customer = customer;
            LineItems = lineItems;
            CalculateTotalOrder();
        }
        public decimal CalculateTotalOrder()
        {
            decimal? total = null;
            if (LineItems != null && LineItems.Count > 0 && Customer?.ShippingAddress?.LocationPricing != null)
            {
                SubTotal = Math.Round(LineItems.Sum(x => x.Price), 2);
                total = SubTotal + ((SubTotal / 100) * Customer?.ShippingAddress?.LocationPricing?.VatRate) +
                    Customer?.ShippingAddress?.LocationPricing?.ShippingFee;
                if (total != null)
                {
                    Total = Math.Round((decimal)total, 2);
                }
            }
            else
                Total = 0;
            return Total;
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
        public Customer Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
    }
}
