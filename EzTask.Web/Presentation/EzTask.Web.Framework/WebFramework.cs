using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using EzTask.Web.Framework.Infrastructures;
using EzTask.Web.Framework.HttpContext;
using EzTask.Business;
using Microsoft.AspNetCore.Mvc;
using EzTask.Web.Framework.Data;

namespace EzTask.Web.Framework
{
    public static class WebFramework
    {
        public static void Register(this IServiceCollection services,
           IConfiguration configuration, IHostingEnvironment env)
        {         
            FrameworkCore.Register(services, configuration);
            BusinessInitializer.Register(services);

            env.RunWebBuilder();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<StaticResources>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<CookiesManager>();
            services.AddSingleton<Interfaces.IWebHostEnvironment, WebHost>();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var mvcBuilder = services.AddMvc(options =>
            {
                // options.Filters.Add(typeof(ExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
    }
}
