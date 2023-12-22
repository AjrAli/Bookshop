using Bookshop.Domain.Common;
using Bookshop.Domain.Extension;

namespace Bookshop.Domain.Entities
{
    public class LineItem : AuditableEntity
    {
        private int _quantity;
        public int Quantity 
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value > Book?.Quantity)
                    _quantity = Book.Quantity;
                else
                    _quantity = value;
                CalculatePrice();
            }
        }
        public decimal Price { get; private set; }
        private LineItem() { }
        private LineItem(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        public LineItem(Book book, int quantity)
        {
            Book = book;
            Quantity = quantity;
        }

        private void CalculatePrice()
        {
            Price = 0;
            // Calculate the Price based on the Quantity and Book's Price.
            Price = (Book != null) ? Book.Price * _quantity : 0;
        }


        // Relationships
        public long? OrderId { get; set; }
        public Order? Order { get; set; }
        public long BookId { get; set; }
        private Book _book;
        public Book Book
        {
            get => LazyLoader.Load(this, ref _book);
            set => _book = value;
        }


        public long? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
    }
}
