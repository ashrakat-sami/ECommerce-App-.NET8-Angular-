using ECommerce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
       

        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress,
             sa => { sa.WithOwner(); });
            builder.HasMany(x => x.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (Status)Enum.Parse(typeof(Status), o));

            builder.Property(m => m.SubTotal)
               .HasColumnType("decimal(18,2)");


        }
    }
    
}
