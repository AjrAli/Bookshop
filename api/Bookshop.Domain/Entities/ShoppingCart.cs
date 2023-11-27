using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class ShoppingCart : AuditableEntity
    {
        public decimal Total { get; private set; }
       


        public void AddItem(Book book, int quantity)
        {
            if (book != null && quantity > 0)
            {
                var item = LineItems.FirstOrDefault(x => x.BookId == book.Id);
                if (item != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    item = new LineItem(book, quantity);
                    LineItems.Add(item);
                }
            }
        }
        public void RemoveItem(Book book, int quantity)
        {
            if (book != null && quantity > 0)
            {
                var item = LineItems.FirstOrDefault(x => x.BookId == book.Id);
                if (item != null)
                {

                    item.Quantity -= quantity;
                    if (item.Quantity <= 0)
                        DeleteItem(book);
                }
            }
        }
        public void DeleteItem(Book book)
        {
            if (book != null)
            {
                var item = LineItems.FirstOrDefault(x => x.BookId == book.Id);
                if (item != null)
                    LineItems.Remove(item);
            }
        }
        public void CalculateTotalWithoutTaxes()
        {
            Total = 0;
            if (LineItems != null && LineItems.Count > 0)
            {
                Total = LineItems.Sum(x => x.Price);
            }
        }

        // Relationships
        public long? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
    }
}
