using ECommerce.Api.Middleware;
using ECommerce.Infrastructure;

namespace ECommerce.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var txt = "CORSPolicy";
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ECommerce API",
                    Version = "v1",
                    Description = "API for ECommerce project"
                });
            });

            builder.Services.InfrastructureConfiguration(builder.Configuration);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200/");
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
                });

            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseMiddleware<ExceptionsMiddleware>();

            app.UseCors(txt);
            app.UseHttpsRedirection();
           
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
