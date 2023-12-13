using Bookshop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Domain.Entities
{
    public class Order : AuditableEntity
    {
        private decimal _total;
        public decimal Total
        {
            get
            {
                return CalculateTotalOrder();
            }
            set
            {
                _total = value;
            }
        }
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
                var totalWithoutTaxes = LineItems.Sum(x => x.Price);
                total = totalWithoutTaxes + ((totalWithoutTaxes / 100) * Customer?.ShippingAddress?.LocationPricing?.VatRate) +
                    Customer?.ShippingAddress?.LocationPricing?.ShippingFee;
                if (total != null)
                {
                    _total = Math.Round((decimal)total, 2);
                }
            }
            return _total;
        }
        public void UpdateStockQuantities()
        {
            Dictionary<Book, int> booksWithInvalidQt = new Dictionary<Book, int>();
            foreach (var lineItem in LineItems)
            {
                if (lineItem.Book != null)
                {
                    if (lineItem.Book.Quantity < lineItem.Quantity)
                    {
                        booksWithInvalidQt.Add(lineItem.Book, lineItem.Quantity);
                    }
                    else
                        lineItem.Book.Quantity -= lineItem.Quantity;

                }
            }
            if (booksWithInvalidQt.Count > 0)
            {
                throw new InsufficientQuantityException(booksWithInvalidQt, "ShoppingCart will be updated with correct quantity available");
            }
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
    }
    public class InsufficientQuantityException : Exception
    {
        public Dictionary<Book, int> BooksKeyWithQtValue { get; set; } = new Dictionary<Book, int>();
        private List<string> Errors { get; set; } = new List<string>();

        public override string Message => $"{base.Message}, (Errors: {string.Join(", ", Errors)})";

        public InsufficientQuantityException(Dictionary<Book, int> booksKeyWithQtValue, string message) : base(message)
        {
            BooksKeyWithQtValue = booksKeyWithQtValue;
            foreach (var keyValuePair in booksKeyWithQtValue)
            {
                Errors.Add($"Book : {keyValuePair.Key.Title}, insufficient quantity => Stock value : {keyValuePair.Key.Quantity}, value ordered {keyValuePair.Value}");
            }
        }
    }

}
