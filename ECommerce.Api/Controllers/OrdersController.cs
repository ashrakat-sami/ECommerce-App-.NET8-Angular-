using ECommerce.Core.DTOs;
using ECommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
         private readonly IOrderService _service;
        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _service.CreateOrdersAsync(orderDto, email);
           // if (order == null) return BadRequest("Problem creating order");
            return Ok(order);
        }
        [HttpGet("GetOrdersForUser")]
        public async Task<IActionResult> GetOrders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var orders = await _service.GetAllOrdersForUserAsync(email);
            return Ok(orders);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _service.GetOrderByIdAsync(id, email);
            return Ok(order);
        }
        [HttpGet("GetDelivery")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _service.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }





    }
}
