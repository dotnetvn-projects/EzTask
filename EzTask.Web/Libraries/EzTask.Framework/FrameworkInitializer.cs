using AutoMapper;
using EzTask.DataAccess;
using EzTask.Framework.ImageHandler;
using EzTask.Framework.Infrastructures;
using EzTask.Framework.Web.Filters;
using EzTask.Framework.Web.HttpContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EzTask.Framework
{
    public static class FrameworkInitializer
    {
        public static void RegisterFramework(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment env)
        {
            env.RunWebBuilder();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<CookiesManager>();
            services.AddScoped<ImageProcessor>();        
            services.AddDbContext<EzTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EzTask")), ServiceLifetime.Scoped);
            services.AddAutoMapper();

            var mvcBuilder = services.AddMvc(options =>
            {
                // options.Filters.Add(typeof(ExceptionFilter));
            });
            mvcBuilder.AddSessionStateTempDataProvider();
            ModuleFinder moduleFinder = new ModuleFinder(env);
            var modules = moduleFinder.Find();

            if (modules.Any())
            {
                foreach (var module in modules)
                {
                    // Register controller from modules
                    mvcBuilder.AddApplicationPart(module.Assembly);
                    mvcBuilder.AddRazorOptions(o =>
                    {
                        o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(module.Assembly.Location));
                    });
                }

                //Register module view location
                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
                });
            }
        }

        public static void ConfigureFramework(this IApplicationBuilder app)
        {
            app.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }

        public static void InvokeComponents<T>(this IServiceProvider services, out T type)
        {
            type = services.GetService<T>();
        }

        public static T InvokeComponents<T>(this IServiceProvider services)
        {
            return services.GetService<T>();
        }

        public static IMapper Mapper => Context.Current.RequestServices.GetService<IMapper>();
    }
}
