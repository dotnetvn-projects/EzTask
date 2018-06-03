using AutoMapper;
using EzTask.DataAccess;
using EzTask.Framework.ImageHandler;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public static class FrameworkCore
{
    public static void Register(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ImageProcessor>();

        services.AddDbContext<EzTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EzTask")), ServiceLifetime.Scoped);
        services.AddAutoMapper();

        var serviceProvider = services.BuildServiceProvider();
        Mapper = serviceProvider.GetService<IMapper>();

        RepositoryInitializer.Register(services);
    }

    public static void InvokeComponents<T>(this IServiceProvider services, out T type)
    {
        type = services.GetService<T>();
    }

    public static T InvokeComponents<T>(this IServiceProvider services)
    {
        return services.GetService<T>();
    }

    internal static IMapper Mapper { get; private set; }
}
