using ECommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTOs
{
    public record OrderDto
    {
        public int DeliveryMethodId { get; set; }

        public string BasketId { get; set; }

        public ShippingAddressDto ShippingAddress { get; set; }
    }

    public record ShippingAddressDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}