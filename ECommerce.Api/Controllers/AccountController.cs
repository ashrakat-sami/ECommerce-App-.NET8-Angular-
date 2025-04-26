using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ECommerce.Api.Helper;

namespace ECommerce.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitofWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDto)
        {
            var result = await work.Auth.RegisterAsync(registerDto);
            if(result != "done")
            {
                return BadRequest(new ApiResponse(400, result));
            }
            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {
            var result = await work.Auth.LoginAsync(loginDTO);
            if(result.StartsWith("Please"))
            {
                return BadRequest(new ApiResponse(400, result));
            }
            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });
            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("Active-Account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ApiResponse(200, "Account activated successfully")) :
                BadRequest(new ApiResponse(400, "Account activation failed"));
        }
        [HttpGet("Send-Email-For-Forget-Password")]
        public async Task<IActionResult>forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ApiResponse(200, "Email has been sent")) :
                BadRequest(new ApiResponse(400, "Email has not been sent"));


        }
    }
}
