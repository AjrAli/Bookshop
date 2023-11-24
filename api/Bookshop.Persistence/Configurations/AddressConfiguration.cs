using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public class AddressConfiguration : BaseEntityConfiguration<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            builder.ToTable("Addresses");

            builder.Property(e => e.Street).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PostalCode).IsRequired().HasMaxLength(10);
            builder.Property(e => e.State).IsRequired().HasMaxLength(100);
            builder.Property(e => e.City).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Country).IsRequired().HasMaxLength(100);
        }
    }
}
