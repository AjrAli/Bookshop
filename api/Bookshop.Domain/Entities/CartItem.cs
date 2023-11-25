using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class CartItem : AuditableEntity
    {
        public int Quantity { get; set; }

        private CartItem() { }
        public CartItem(int quantity)
        {
            Quantity = quantity;
        }



        // Relationships
        public long? BookId { get; set; }
        public Book? Book { get; set; }
        public long? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
    }
}
