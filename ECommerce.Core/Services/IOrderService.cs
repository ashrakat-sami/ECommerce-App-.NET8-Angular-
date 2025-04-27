using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services
{
    public interface IOrderService
    {
        Task<Orders> CreateOrdersAsync(OrderDto orderDto,string BuyerEmail);

        Task<IReadOnlyList<OrderToReturnDto>> GetAllOrdersForUserAsync(string buyerEmail);

        Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();


    }
}
