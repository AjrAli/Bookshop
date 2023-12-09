using Bookshop.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookshop.Persistence.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Auto number primary key
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            // CreatedBy
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedBy).Metadata.GetBeforeSaveBehavior();

            // CreatedDate
            builder.Property(e => e.CreatedDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(e => e.CreatedDate).Metadata.GetBeforeSaveBehavior();

            // LastModifiedBy
            builder.Property(e => e.LastModifiedBy).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastModifiedBy).Metadata.GetBeforeSaveBehavior();

            // LastModifiedDate
            builder.Property(e => e.LastModifiedDate).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(e => e.LastModifiedDate).Metadata.GetBeforeSaveBehavior();
        }
    }
}
