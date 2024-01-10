using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.ToTable("Orders");

            builder.Property(e => e.Total).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.SubTotal).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.MethodOfPayment).IsRequired().HasConversion<string>();
            builder.Property(e => e.DateOrder).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.StatusOrder).IsRequired().HasConversion<string>();


            // Relationships
            builder.HasMany(e => e.LineItems).WithOne(e => e.Order)
                   .HasForeignKey(e => e.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
