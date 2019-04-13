using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Business
{
    public static class BusinessInitializer
    {
        private static IServiceCollection _services;

        public static void Register(IServiceCollection services)
        {
            _services = services;
            _services.AddScoped<EzTaskBusiness>();
            _services.AddScoped<AccountBusiness>();
            _services.AddScoped<ProjectBusiness>();
            _services.AddScoped<SkillBusiness>();
            _services.AddScoped<PhaseBusiness>();
            _services.AddScoped<TaskBusiness>();
            _services.AddScoped<NotificationBusiness>();
            _services.AddScoped<ToDoListBusiness>();
        }

        public static ServiceProvider ServiceProvider
        {
            get
            {
                return _services.BuildServiceProvider();
            }
        }
    }
}
