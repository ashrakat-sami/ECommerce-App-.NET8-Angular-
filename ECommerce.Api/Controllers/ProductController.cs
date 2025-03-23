using AutoMapper;
using ECommerce.Api.Helper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products=await work.ProductRepository.GetAllAsync(x=>x.Category,x=>x.Photos);
                if(products == null) return BadRequest(new ApiResponse(400));
                var productsToReturn = mapper.Map<List<ProductDto>>(products);
                return Ok(productsToReturn);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
