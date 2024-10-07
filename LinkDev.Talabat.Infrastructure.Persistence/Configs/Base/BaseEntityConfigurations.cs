﻿using LinkDev.Talabat.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Configs.Base
{
    public class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

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
