using EzTask.DataAccess;
using EzTask.Framework.Web.HttpContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework
{
    public static class Startup
    {
        public static void RegisterFrameworkService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<SessionManager>();
            services.AddSingleton<CookiesManager>();

            services.AddDbContext<EzTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EzTask")),
               ServiceLifetime.Scoped);
        }

        public static void ConfigureFramework(this IApplicationBuilder app)
        {
            app.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }
    }
}
