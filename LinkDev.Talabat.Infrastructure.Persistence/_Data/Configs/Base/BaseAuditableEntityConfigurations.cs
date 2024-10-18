using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Configs.Base
{
    public class BaseAuditableEntityConfigurations<TEntity, TKey> : BaseEntityConfigurations<TEntity, TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(E => E.Id)
             .IsRequired()
             .ValueGeneratedOnAdd();

            builder.Property(E => E.CreatedBy)
                .IsRequired();
            builder.Property(E => E.CreatedOn)
                .IsRequired()
               /* .HasDefaultValue("GetUTCDate()")*/;


            builder.Property(E => E.LastModifiedBy)
               .IsRequired();
            builder.Property(E => E.LastModifiedOn)
                .IsRequired()
                /*.HasDefaultValue("GetUTCDate()")*/;
        }
    }
}
