using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class CustomerCart
    {
        public CustomerCart()
        {
            
        }
        public CustomerCart(string id)
        {
            Id = id;
        }
        // this will be stored in the database(redis)
        public string Id { get; set; } // key
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();//value
    }
}
