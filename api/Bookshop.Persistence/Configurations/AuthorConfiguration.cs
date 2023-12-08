using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class AuthorConfiguration : BaseEntityConfiguration<Author>
    {
        public override void Configure(EntityTypeBuilder<Author> builder)
        {
            base.Configure(builder);

            builder.ToTable("Authors");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.About).IsRequired().HasMaxLength(100);

            // Relationships
            builder.HasMany(e => e.Books).WithOne(e => e.Author)
                   .HasForeignKey(e => e.AuthorId).IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
