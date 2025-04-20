using AutoMapper;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
 
    public class BugController : BaseController
    {
        public BugController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("notFound")]
        public async Task<IActionResult> GetNotFound()
        {
            var thing =await work.CategoryRepository.GetByIdAsync(100);
            if (thing == null) return NotFound();
            return Ok(thing);
        }
        [HttpGet("serverError")]
        public async Task<IActionResult> GetServerError()
        {
            var thing = await work.CategoryRepository.GetByIdAsync(100);
            thing.Name= "Error";
            return Ok(thing);
        }
        [HttpGet("badRequest/{Id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {

            return Ok();
        }

        [HttpGet("badRequest")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }


    }
}
