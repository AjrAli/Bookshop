using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class CartItemConfiguration : BaseEntityConfiguration<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("CartItems");

            builder.Property(e => e.Quantity).IsRequired();

            // Add check constraint using raw SQL
            builder.HasCheckConstraint("CK_QuantityItem_MaxValue", "[Quantity] <= 100");
        }
    }
}
