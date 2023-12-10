using Bookshop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Persistence.Configurations
{
    public class LocationPricingConfiguration : BaseEntityConfiguration<LocationPricing>
    {
        public override void Configure(EntityTypeBuilder<LocationPricing> builder)
        {
            base.Configure(builder);

            builder.ToTable("LocationPricings");

            builder.Property(e => e.VatRate).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.ShippingFee).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.Country).IsRequired().HasMaxLength(100);

        }
    }
}
