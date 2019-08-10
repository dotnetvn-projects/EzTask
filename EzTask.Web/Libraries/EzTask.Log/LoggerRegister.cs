using EzTask.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using EzTask.Log;

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
