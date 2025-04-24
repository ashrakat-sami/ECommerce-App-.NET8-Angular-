using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Order;

namespace ECommerce.Api.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>()
                .ReverseMap();
            CreateMap<Orders, OrderToReturnDto>()
                
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s => s.DeliveryMethod.Name))
                .ReverseMap();


            CreateMap<OrderItem, OrderItemDto>()
               .ReverseMap();

        }
    }
}
