using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class BookOrder : AuditableEntity
    {
        public int QuantityOrder { get; set; }
        private BookOrder() { }
        public BookOrder(int quantityOrder)
        {
            QuantityOrder = quantityOrder;
        }



        // Relationships
        public long? OrderId { get; set; }
        public Order? Order { get; set; }
        public long BookId { get; set; }
        public Book? Book { get; set; }

    }
}
