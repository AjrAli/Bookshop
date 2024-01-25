using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class LineItemConfiguration : BaseEntityConfiguration<LineItem>
    {
        public override void Configure(EntityTypeBuilder<LineItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("LineItems");
            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.Quantity).IsRequired();

            // Add check constraint using raw SQL
            builder.HasCheckConstraint("CK_Quantity_MaxValue", "[Quantity] <= 100");
            // Relationships
            builder.HasOne(e => e.Book)
                   .WithMany()
                   .HasForeignKey(e => e.BookId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
