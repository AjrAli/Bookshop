using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class ShoppingCart : AuditableEntity
    {
        public decimal Total { get; private set; }

        // Relationships
        public long? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

        public void CalculateTotal()
        {
            Total = 0;
            if (LineItems != null && LineItems.Count > 0)
            {
                Total = LineItems.Sum(x => x.Price);
            }
        }
    }
}
