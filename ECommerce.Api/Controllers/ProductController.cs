using AutoMapper;
using ECommerce.Api.Helper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ECommerce.Api.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductParam productParam)
        {
            try
            {
                var products=await work.ProductRepository
                    .GetAllAsync(productParam);

                var totalCount = await work.ProductRepository.CountAsync();

                return Ok(new Pagination<ProductDto>(productParam.PageNumber,productParam.PageSize,totalCount,products));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await work.ProductRepository.GetByIdAsync(id, x => x.Category, x => x.Photos); 
                var result=mapper.Map<ProductDto>(product);
                if(result == null) return BadRequest(new ApiResponse(400));
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add(AddProductDto productDto)
        {
            try
            {

               await work.ProductRepository.AddAsync(productDto);
                return Ok(new ApiResponse(200));

               
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(updateProductDto);
                return Ok(new ApiResponse(200));

            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.ProductRepository.GetByIdAsync(id, x => x.Photos, x => x.Category);
                await work.ProductRepository.DeleteAsync(product);

                return Ok(new ApiResponse(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400, ex.Message));

            }
        }

        


    }
}
