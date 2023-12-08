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
        public void UpdateLineItem(Book book, int quantity)
        {
            if (book != null)
            {
                var item = LineItems.FirstOrDefault(x => x.BookId == book.Id);
                if (item != null)
                {
                    item.Quantity = quantity;
                    if (item.Quantity <= 0)
                        LineItems.Remove(item);
                }
                else if(quantity > 0)
                {
                    item = new LineItem(book, quantity);
                    LineItems.Add(item);
                }
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
