using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities.Order
{
   public class Orders:BaseEntity<int>
    {
        public Orders()
        {
        }
        public Orders(string buyerEmail, decimal subTotal,  ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
          
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
           
        }

        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; } 

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public  ShippingAddress ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        public Status Status { get; set; } = Status.pending; //If payment is succeeded or not


        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }

    }
}
