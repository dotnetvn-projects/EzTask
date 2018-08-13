using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Business
{
    public static class BusinessInitializer
    {
        private static IServiceCollection _services;

        public static void Register(IServiceCollection services)
        {
            _services = services;
            _services.AddTransient<EzTaskBusiness>();
            _services.AddTransient<AccountBusiness>();
            _services.AddTransient<ProjectBusiness>();
            _services.AddTransient<SkillBusiness>();
            _services.AddTransient<PhraseBusiness>();
            _services.AddTransient<TaskBusiness>();
            _services.AddTransient<AttachmentBusiness>();
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
