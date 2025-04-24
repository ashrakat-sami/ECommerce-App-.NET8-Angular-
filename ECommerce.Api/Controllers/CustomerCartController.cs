using AutoMapper;
using Azure;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    public class CustomerCartController : BaseController
    {
        public CustomerCartController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("Get-Cart-Item/{id}")]
        public async Task<IActionResult> GetCartItem(string id)
        {
            var result = await work.CustomerCart.GetCartAsync(id);
            if (result is null)
            {
                return Ok(new CustomerCart());
            }
            return Ok(result);
        }

        [HttpPost("Add-Cart-Item")]
        public async Task<IActionResult> AddCartItem(CustomerCart cart)
        {
            var _cart = await work.CustomerCart.UpdateCartAsync(cart);
            return Ok(_cart);
        }   

        [HttpDelete("Delete-Cart-Item{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var result = await work.CustomerCart.DeleteCartAsync(id);
            if (result)
            {
                return Ok("Item Deleted");
            }
            else
            {
                return BadRequest("Item Not Found");
            }
        }
    }
}
