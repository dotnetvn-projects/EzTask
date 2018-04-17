using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Business
{
    public static class Startup
    {
        public static void RegisterBusiness(this IServiceCollection services)
        { 
            services.AddTransient<EzTaskBusiness>();
            services.AddTransient<AccountBusiness>();
            services.AddTransient<ProjectBusiness>();
        }
    }
}
