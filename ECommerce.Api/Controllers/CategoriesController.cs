using AutoMapper;
using ECommerce.Api.Helper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await work.CategoryRepository.GetAllAsync();
                if (categories == null)
                    return BadRequest(new ApiResponse(400));

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return BadRequest(new ApiResponse(400,$"categroy with id:{id} is not found"));
                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDto);

                await work.CategoryRepository.AddAsync(category);
                return Ok(new {message="category has been added"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Updaye(CategoryUpdateDto categoryDto)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDto);
                await work.CategoryRepository.UpdateAsync(category);
                return Ok(new { message = "Category has been updated" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               await work.CategoryRepository.DeleteAsync(id);
                return Ok(new { message = "Category has been deleted" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
