using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService
{
    public static class MessageServiceRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MesageServiceCenter>();
        }
    }
}
