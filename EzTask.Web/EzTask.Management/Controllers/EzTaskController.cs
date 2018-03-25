using System;

using EzTask.Interfaces;
using EzTask.MainBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Management.Controllers
{
    public class EzTaskController : Controller
    {
        protected static EzTaskBusiness EzTask { get; private set; }

        public EzTaskController(IServiceProvider serviceProvider)
        {
            if (EzTask == null)
            {
                var service = serviceProvider.GetService<IEzTaskBusiness>();
                if (service != null)
                {
                    EzTask = service as EzTaskBusiness;
                }
            }          
        }
    }
}