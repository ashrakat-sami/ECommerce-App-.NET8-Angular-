using AutoMapper;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitofWork work;
        protected readonly IMapper mapper;

        public BaseController(IUnitofWork work,IMapper mapper )
        {
            this.work = work;
            this.mapper = mapper;
        }
    }
}
