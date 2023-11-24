using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class BookOrder : AuditableEntity
    {
        public int QuantityOrder { get; set; }
        private BookOrder() { }
        public BookOrder(int quantityOrder, long orderId, long bookId)
        {
            QuantityOrder = quantityOrder;
            OrderId = orderId;
            BookId = bookId;
        }



        // Relationships
        public long OrderId { get; set; }
        public Order? Order { get; set; }
        public long BookId { get; set; }
        public Book? Book { get; set; }

    }
}
