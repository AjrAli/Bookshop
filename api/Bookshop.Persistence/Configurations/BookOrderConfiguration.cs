using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class BookOrderConfiguration : BaseEntityConfiguration<BookOrder>
    {
        public override void Configure(EntityTypeBuilder<BookOrder> builder)
        {
            base.Configure(builder);

            builder.ToTable("BookOrders");

            builder.Property(e => e.QuantityOrder).IsRequired();

            // Add check constraint using raw SQL
            builder.HasCheckConstraint("CK_QuantityOrder_MaxValue", "[QuantityOrder] <= 100");
        }
    }
}
