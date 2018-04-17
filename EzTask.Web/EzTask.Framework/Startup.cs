using EzTask.DataAccess;
using EzTask.Repository;
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

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EzTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EzTask")),
               ServiceLifetime.Scoped);
        }

        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddTransient<ProjectRepository>();
        }

        public static void RegisterBusiness(this IServiceCollection services)
        {

        }
    }
}
