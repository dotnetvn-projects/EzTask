using EzTask.Business;
using EzTask.Interface;
using EzTask.Log;
using EzTask.Model;
using EzTask.Plugin.MessageService;
using EzTask.Web.Framework.Data;
using EzTask.Web.Framework.Filters;
using EzTask.Web.Framework.Infrastructures;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace EzTask.Web.Framework
{
    public static class WebFramework
    {
        public static void Register(this IServiceCollection services,
           IConfiguration configuration, IWebHostEnvironment env)
        {
            FrameworkCore.Register(services, configuration);
            ModelRegister.Register(services);
            BusinessRegister.Register(services);
            MessageServiceRegister.Register(services, configuration);
            LoggerRegister.Register(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<StaticResources>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<CookiesManager>();
            services.AddSingleton<IWebEnvironment, WebHost>();
            services.AddTransient<ILanguageLocalization, LanguageLocalization>();
            services.AddTransient<ViewRender>();
            services.AddSingleton<IAccountContext, AccountContext>();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMemoryCache();
            services.AddResponseCompression();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var mvcBuilder = services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalFilter));
                // options.Filters.Add(typeof(ExceptionFilter));
            }).AddRazorRuntimeCompilation();


            mvcBuilder.AddSessionStateTempDataProvider();

            ModuleFinder moduleFinder = new ModuleFinder(env);
            var modules = moduleFinder.Find();

            if (modules.Any())
            {
                foreach (var module in modules)
                {
                    // Register controller from modules
                    mvcBuilder.AddApplicationPart(module.Assembly);
                }

                //Register module view location
                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
                });
            }
            FrameworkCore.ServiceProvider = services.BuildServiceProvider();
        }

        public static void ConfigureFramework(this IApplicationBuilder app)
        {
            app.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(),
                app.ApplicationServices.GetRequiredService<IWebEnvironment>(),
                app.ApplicationServices.GetRequiredService<SessionManager>(),
                app.ApplicationServices.GetRequiredService<CookiesManager>(),
                app.ApplicationServices.GetRequiredService<ILanguageLocalization>(),
                app.ApplicationServices.GetRequiredService<IAccountContext>());
        }
    }
}
