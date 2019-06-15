using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Model
{
    public static class ModelRegister
    {
        private static IServiceCollection _services;
        public static void Register(IServiceCollection services)
        {
            _services = services;

            services.AddAutoMapper();

            var serviceProvider = services.BuildServiceProvider();
            Mapper = serviceProvider.GetService<IMapper>();
        }

        internal static IMapper Mapper { get; private set; }
    }
}
