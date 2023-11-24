using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class ShoppingCart : AuditableEntity
    {
        public decimal Total { get; set; }

        private ShoppingCart() { }

        public ShoppingCart(decimal total,
                     long customerId)
        {
            Total = total;
            CustomerId = customerId;
        }

        // Relationships
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
