using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class ShoppingCart : AuditableEntity
    {
        private decimal _total;
        public decimal Total
        {
            get
            {
                return CalculateSubtotal();
            }
            set
            {
                _total = value;
            }

        }
        private ShoppingCart() { }
        public ShoppingCart(Customer customer, ICollection<LineItem>? lineItems = null)
        {
            Customer = customer;
            LineItems = lineItems ?? new List<LineItem>();
            if (LineItems.Count > 0)
                CalculateSubtotal();

        }
        public void UpdateCartItem(Book book, int quantity)
        {
            if (book == null)
            {
                return;
            }

            var existingItem = LineItems?.FirstOrDefault(x => x.BookId == book.Id);

            if (existingItem != null)
            {
                UpdateExistingItem(existingItem, quantity);
            }
            else
            {
                AddNewItem(book, quantity);
            }
            CalculateSubtotal();
        }
        private void UpdateExistingItem(LineItem existingItem, int quantity)
        {
            if (quantity <= 0)
            {
                LineItems?.Remove(existingItem);
            }
            else
            {
                existingItem.Quantity = quantity;
            }
        }
        private void AddNewItem(Book book, int quantity)
        {
            if (quantity > 0)
            {
                var newItem = new LineItem(book, quantity);
                LineItems?.Add(newItem);
            }
        }

        public decimal CalculateSubtotal()
        {
            decimal? total = null;
            if (LineItems != null && LineItems.Count > 0)
            {
                total = LineItems.Sum(x => x.Price);
                if (total != null)
                {
                    _total = Math.Round((decimal)total, 2);
                }
            }
            else
                _total = 0;
            return _total;
        }
        public int TotalItems()
        {
            return LineItems?.Sum(x => x.Quantity) ?? 0;
        }
        // Relationships
        public long? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<LineItem>? LineItems { get; set; }
    }
}
