using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class ShoppingCartConfiguration : BaseEntityConfiguration<ShoppingCart>
    {
        public override void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            base.Configure(builder);

            builder.ToTable("ShoppingCarts");

            builder.Property(e => e.Total).IsRequired().HasColumnType("decimal(18,2)");


            // Relationships
            builder.HasMany(e => e.CartItems).WithOne(e => e.ShoppingCart)
                   .HasForeignKey(e => e.ShoppingCartId).IsRequired();
            builder.HasOne(e => e.Customer)
                   .WithOne()
                   .HasForeignKey<ShoppingCart>(e => e.CustomerId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
