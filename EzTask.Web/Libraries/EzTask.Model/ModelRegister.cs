using AutoMapper;
using EzTask.Framework.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Model
{
    public static class ModelRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddAutoMapper(AssemblyUtilities.GetAssemblies());

            var serviceProvider = services.BuildServiceProvider();
            Mapper = serviceProvider.GetService<IMapper>();
        }

        internal static IMapper Mapper { get; private set; }
    }
}
