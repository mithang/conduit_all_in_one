using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Business.AutoMapping;
using Conduit.Business.Helpers;
using Conduit.Business.Managers.EntityFramework;
using Conduit.Business.Services;
using Conduit.Common.DataAccess;
using Conduit.Data;
using Conduit.Data.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Conduit.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Allow-Origin ve diğer ayarlar için
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("x-pagination")
                        .AllowCredentials();
                    });
            });

            services.AddDbContext<ConduitContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Đảm bảo bạn không theo dõi. Nhưng mất hiệu suất.
            });

            //AutoMapping dùng file cấu hình, không cần thiết khai báo trong starttup
            var mapConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappingProfileConfiguration());
            });
            var mapper = mapConfig.CreateMapper();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepositoryBase<>));
            services.AddScoped<ITagServices, EfTagManager>();
            services.AddScoped<IArticleServices, EfArticleManager>();
            services.AddScoped<IUserServices, EfUserManager>();
            services.AddScoped<ITokenServices, EfUserManager>();
            services.AddScoped<IFollowedUserServices, EfFollowedUserManager>();
            services.AddScoped<IArticleTagsServices, EfArticleTagManager>();
            services.AddScoped<IArticleFavoriteServices, EfArticleFavoriteManager>();
            services.AddScoped<ICommentSercices, EfCommentManager>();
            services.AddScoped<INoteServices, EfNoteManager>();


            services.AddScoped<ILibraryRepository, LibraryRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton(mapper); //Mapping Dependency
            services.AddMvc()
                .AddJsonOptions(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);//Include 'un çalışması için gerekli

            //Swashbuckle.AspNetCore
            //Link truy cập --> localhost:{port}/swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("ConduitApi", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Conduit API", Version = "v1" });
                //Nếu route bị trùng thì lấy route đầu tiên
                //option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //Thêm authentication vào swagger
                option.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                option.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });

            });

            //Internal Server Error /swagger/v1/swagger.json -> Nếu bạn gặp lỗi: các thẻ như [HttpGet] bị thiếu. Mọi phương thức đều phải được gắn thẻ


            #region - JWT TOKEN - THEO THUẬT TOÁN: HS384
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //Lấy dữ liệu cầu hình từ appsetting.json
                    ValidateAudience = (bool)Convert.ChangeType(Configuration["JwtTokenConfig:ValidateAudience"], typeof(bool)),
                    ValidAudience = (string)Convert.ChangeType(Configuration["JwtTokenConfig:ValidAudience"], typeof(string)),
                    ValidateIssuer = (bool)Convert.ChangeType(Configuration["JwtTokenConfig:ValidateIssuer"], typeof(bool)),
                    ValidIssuer = (string)Convert.ChangeType(Configuration["JwtTokenConfig:ValidIssuer"], typeof(string)),
                    ValidateLifetime = (bool)Convert.ChangeType(Configuration["JwtTokenConfig:ValidateLifetime"], typeof(bool)),
                    ValidateIssuerSigningKey = (bool)Convert.ChangeType(Configuration["JwtTokenConfig:ValidateIssuerSigningKey"], typeof(bool)),
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes((string)Convert.ChangeType(Configuration["JwtTokenConfig:IssuerSigningKey"], typeof(string))))
                };
             
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        //Phải làm gì khi thành công
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        //Phải làm gì khi bị lỗi
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ConduitContext context )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Đảm bảo db sẽ được tạo trước khi dùng, Chú ý chỉ chạy một lần thôi rồi đóng lại
            context.EnsureSeedDataForContext();

            //Dùng Cors với tên AllowAll
            app.UseCors("AllowAll");

            //Chú ý: Add Swagger phải yêu cầu dùng UseAuthentication mới chạy được
            //Dùng Middleware Authentication
            app.UseAuthentication();

            //Dùng Middleware Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/ConduitApi/swagger.json", "Conduit API");

            });

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=Home}/{action=Index}/{id?}");
//
//                routes.MapSpaFallbackRoute(
//                    name: "spa-fallback",
//                    defaults: new { controller = "Home", action = "Index" });
//            });
            app.UseMvc();
        }
    }
}
