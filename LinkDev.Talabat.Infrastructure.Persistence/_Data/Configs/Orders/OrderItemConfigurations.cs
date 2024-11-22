using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence._Data.Configs.Base;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Configs.Orders
{
    internal class OrderItemConfigurations : BaseAuditableEntityConfigurations<OrderItem,int>
    {

        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);
            builder.OwnsOne(item => item.Product, product => product.WithOwner());
            builder.Property(item => item.Price)
                .HasColumnType("decimal(8,2)");

        }
    }
}
