using EzTask.Interface;
using EzTask.Plugin.MessageService.Data.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService
{
    public static class MessageServiceRegister
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailServerSettings>(x => configuration.GetSection("MailServerSettings").Bind(x));
            services.AddSingleton<IMessageCenter, MesageServiceCenter>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
