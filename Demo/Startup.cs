using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.Use( async (context,next) =>
            {
                //Gán giá trị cho middle
                context.Items["data"] = "Value";
                await context.Response.WriteAsync("Run 1 <br/>");
                await next.Invoke();
            });
            app.UseMyCustomMiddleware();
            app.Run(async (context) =>
            {
                //Nhận giá trị từ context trước
                await context.Response.WriteAsync("Run 2 "+context.Items["data"]+"<br/>");               
            });

        }
    }
}
