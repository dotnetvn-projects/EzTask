using EzTask.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Log
{
    public static class LoggerRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
        }
    }
}
