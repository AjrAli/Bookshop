using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class LineItem : AuditableEntity
    {
        public int Quantity { get; set; }
        public decimal Price { get; private set; }
        private LineItem() { }
        public LineItem(int quantity)
        {
            Quantity = quantity;
        }



        // Relationships
        public long? OrderId { get; set; }
        public Order? Order { get; set; }
        public long BookId { get; set; }
        private Book? _book;
        public Book? Book
        {
            get { return _book; }
            set
            {
                if (value != null)
                {
                    _book = value;
                    CalculatePrice();
                }
            }
        }
        public long? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        private void CalculatePrice()
        {
            Price = 0;
            // Calculate the Price based on the Quantity and Book's Price.
            Price = (_book != null) ? _book.Price * Quantity : 0;
        }
    }
}
