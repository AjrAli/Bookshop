using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class BookConfiguration : BaseEntityConfiguration<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.ToTable("Books");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.Publisher).IsRequired();
            builder.Property(e => e.Isbn).IsRequired();
            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.Quantity).IsRequired();
            builder.Property(e => e.PageCount).IsRequired();
            builder.Property(e => e.Dimensions).IsRequired();
            builder.Property(e => e.ImageUrl).IsRequired();
            builder.Property(e => e.Language).IsRequired().HasConversion<string>();
            builder.Property(e => e.PublishDate).IsRequired().HasColumnType("datetime2");

            // Add check constraint using raw SQL
            //builder.HasCheckConstraint("CK_Quantity_MaxValue", "[Quantity] <= 1000");
        }
    }
}
