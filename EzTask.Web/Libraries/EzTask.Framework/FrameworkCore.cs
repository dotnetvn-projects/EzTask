using EzTask.DataAccess;
using EzTask.Framework.ImageHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public static class FrameworkCore
{
    /// <summary>
    /// register framework core
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void Register(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ImageProcessor>();

        services.AddDbContext<EzTaskDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EzTask")),
                ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Invoke component and give an output type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="type"></param>
    public static void InvokeComponents<T>(this IServiceProvider services, out T type)
    {
        type = services.GetService<T>();
    }

    /// <summary>
    /// Invoke component and return an output type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static T InvokeComponents<T>(this IServiceProvider services)
    {
        return services.GetService<T>();
    }

    /// <summary>
    /// Global service 
    /// </summary>
    public static IServiceProvider ServiceProvider { get; set; }
}
