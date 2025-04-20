using ECommerce.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow=TimeSpan.FromSeconds(30);

        public ExceptionsMiddleware(RequestDelegate next,  IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);
                if (isReuestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many requests ,try again later");
                    await context.Response.WriteAsJsonAsync(response);
                    
                }
                await _next(context);
            }
            catch (Exception ex)
            {
               context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment()?
                    new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace)
                    : new ApiException((int)HttpStatusCode.InternalServerError,ex.Message); //Production

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }
        }

        private bool isReuestAllowed(HttpContext context)
        {
            var ip=context.Connection.RemoteIpAddress.ToString();
            var cashKey = $"Rate:{ip}";
            var dateNow= DateTime.Now;

            var (timeStamp, count) = _memoryCache.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return(timeStamp: dateNow, count: 0);
            }
            );

            if(dateNow - timeStamp < _rateLimitWindow)
            {
                if (count > 8)
                {
                    return false;
                }
                _memoryCache.Set(cashKey, (timeStamp, count + 1),_rateLimitWindow);

            }
            else
            {
                _memoryCache.Set(cashKey, (timeStamp, count), _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";


            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            context.Response.Headers["Permissions-Policy"] = "geolocation=(self), microphone=()";
            context.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:4200";


        }

  
    }
}
