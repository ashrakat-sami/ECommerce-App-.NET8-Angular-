using ECommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOs
{
    public record OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }


        public DateTime OrderDate { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }

        public string Status { get; set; }
    }
    public record OrderItemDto
    {
        public int ProductItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
    }
}
