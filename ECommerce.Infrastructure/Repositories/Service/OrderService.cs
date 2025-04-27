using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.Service
{
    class OrderService : IOrderService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(AppDbContext context, IUnitofWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Orders> CreateOrdersAsync(OrderDto orderDto, string BuyerEmail)
        {
            var basket =await _unitOfWork.CustomerCart.GetCartAsync(orderDto.BasketId);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.CartItems)
            {
              var product= await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(product.Id, item.Quantity, item.Price, item.image, product.Name);
                orderItems.Add(orderItem);

            }
            var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == orderDto.DeliveryMethodId);
            var subtotal = orderItems.Sum(x => x.Price * x.Quantity);
            var ship=_mapper.Map<ShippingAddress>(orderDto.ShippingAddress);


            var order = new Orders(BuyerEmail, subtotal, ship, deliveryMethod, orderItems);

            await _context.Orders.AddAsync(order);
             await _context.SaveChangesAsync();
           await _unitOfWork.CustomerCart.DeleteCartAsync(orderDto.BasketId);
            return order;
        }

        public async Task<IReadOnlyList<OrderToReturnDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var orders =await _context.Orders
                .Where(m => m.BuyerEmail == buyerEmail)
                .Include(m => m.OrderItems)
                .Include(m => m.DeliveryMethod)
                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        
          => await _context.DeliveryMethods.AsNoTracking().ToListAsync();
        
        

        public async Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
        var order=await _context.Orders.Where(m=>m.Id== id && m.BuyerEmail == buyerEmail)
            .Include(m => m.OrderItems)
            .Include(m => m.DeliveryMethod)
            .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDto>(order);
            return result;   
        }
    }
}
