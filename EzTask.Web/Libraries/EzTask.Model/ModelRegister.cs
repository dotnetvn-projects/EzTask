using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Model
{
    public static class ModelRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddAutoMapper();

            var serviceProvider = services.BuildServiceProvider();
            Mapper = serviceProvider.GetService<IMapper>();
        }

        internal static IMapper Mapper { get; private set; }
    }
}
