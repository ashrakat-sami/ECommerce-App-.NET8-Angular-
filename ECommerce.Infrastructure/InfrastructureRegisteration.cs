using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // apply unit of work pattern
            services.AddScoped<IUnitofWork, UnitofWork>();
            //Register Email Sender
            services.AddScoped<IEmailService, EmailService>();
            //register token generator
            services.AddScoped<IGenerateToken, GenerateToken>();
            // apply redis connection
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var configurations = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(configurations);
            });

            //Register IOrder Service
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddSingleton<IImageManagementService, ImageManagementService>();

            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("connection"));
            });

            // Authentication and Authorization with Jwtbearer
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication( op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie( o =>
            {
                o.Cookie.Name = "token";
                o.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            }).AddJwtBearer(op =>
            {
                op.RequireHttpsMetadata = false;
                op.SaveToken = true;
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };
                op.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
