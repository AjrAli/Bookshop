using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("Customers");

            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);


            // Relationships
            builder.HasOne(e => e.ShippingAddress)
                   .WithMany()
                   .HasForeignKey(e => e.ShippingAddressId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.BillingAddress)
                   .WithMany()
                   .HasForeignKey(e => e.BillingAddressId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.IdentityData)
                   .WithOne()
                   .HasForeignKey<Customer>(e => e.IdentityUserDataId).IsRequired();
            builder.HasOne(e => e.ShoppingCart)
                   .WithOne()
                   .HasForeignKey<Customer>(e => e.ShoppingCartId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(e => e.Orders).WithOne(e => e.Customer)
                   .HasForeignKey(e => e.CustomerId).IsRequired();

        }
    }
}
