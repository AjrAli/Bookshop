using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.ToTable("Categories");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
            builder.Property(e => e.IsVisible).IsRequired().HasDefaultValue(true);

            // Relationships
            builder.HasMany(e => e.Books).WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryId).IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
