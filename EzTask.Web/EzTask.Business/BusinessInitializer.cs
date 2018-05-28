using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Business
{
    public static class BusinessInitializer
    {
        public static void RegisterBusiness(this IServiceCollection services)
        { 
            services.AddTransient<EzTaskBusiness>();
            services.AddTransient<AccountBusiness>();
            services.AddTransient<ProjectBusiness>();
            services.AddTransient<SkillBusiness>();
            services.AddTransient<PhraseBusiness>();
            services.AddTransient<TaskBusiness>();
        }
    }
}
