using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class CommentConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Comments");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            builder.Property(e => e.Rating).IsRequired();
            builder.Property(e => e.DateComment).IsRequired().HasColumnType("datetime2");

            // Add check constraint using raw SQL
            builder.HasCheckConstraint("CK_Rating_MaxValue", "[Rating] <= 5");
            builder.HasCheckConstraint("CK_Rating_MinValue", "[Rating] > 0");

            // Relationships
            builder.HasOne(e => e.Customer)
                   .WithMany()
                   .HasForeignKey(e => e.CustomerId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
