using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Demo
{
    // Class custom middleware cần xử lí
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        //Không nên dùng phương thức này
        //public Task Invoke(HttpContext httpContext)
        //{
        //    return _next(httpContext);
        //}
        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Response.WriteAsync("Chạy custom middleware "+ httpContext.Items["data"]+"<br/>");
            await _next.Invoke(httpContext);
            await httpContext.Response.WriteAsync("Hoàn thành custom middleware <br/>");
        }
    }

    // Mở rộng phương thức IApplicationBuilder để có thể dùng trong Startup.cs
    public static class MyCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
