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
                return CalculateTotalWithoutTaxes();
            }
            set
            {
                _total = value;
            }
        
        }
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
                CalculateTotalWithoutTaxes();
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
                CalculateTotalWithoutTaxes();
            }
        }
        public void DeleteItem(Book book)
        {
            if (book != null)
            {
                var item = LineItems.FirstOrDefault(x => x.BookId == book.Id);
                if (item != null)
                    LineItems.Remove(item);
                CalculateTotalWithoutTaxes();
            }
        }
        public decimal CalculateTotalWithoutTaxes()
        {
            decimal? total = null;
            if (LineItems != null && LineItems.Count > 0)
            {
                total = LineItems.Sum(x => x.Price);
                if (total != null)
                {
                    _total = (decimal)total;
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
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
    }
}
