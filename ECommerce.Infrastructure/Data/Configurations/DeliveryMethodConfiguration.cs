using ECommerce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price)
               .HasColumnType("decimal(18,2)");
            builder.HasData(new DeliveryMethod
            {
                Id = 1,
                Name = "Standard",
                Price = 10,
                Descripton = "Standard delivery",
                DeliveryTime = "within a week"
            },
             new DeliveryMethod
             {
                 Id = 2,
                 Name = "Fast",
                 Price = 15,
                 Descripton = "Faster delivery",
                 DeliveryTime = "5 days"
             });
        }
    }
}
