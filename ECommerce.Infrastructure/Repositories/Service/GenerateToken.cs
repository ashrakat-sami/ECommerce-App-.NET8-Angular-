using ECommerce.Core.Entities;
using ECommerce.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.Service
{
    public class GenerateToken : IGenerateToken    
    {
        private readonly IConfiguration configuration;
        public GenerateToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user)
        {
            // Create a new token handler
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            // Create a new token descriptor
            var security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(security);
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials,
                NotBefore = DateTime.Now
            };
            // Create a new token handler
            JwtSecurityTokenHandler  handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
