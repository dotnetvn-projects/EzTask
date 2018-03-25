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

        /// <summary>
        /// Page title
        /// </summary>
        protected string PageTitle
        {
            get { return "EzTask - "+ ViewData["Title"]?.ToString(); }
            set { ViewData["Title"] = value; }
        }

        /// <summary>
        /// ErrorMessage
        /// </summary>
        protected string ErrorMessage
        {
            get { return ViewData["error"]?.ToString(); }
            set { ViewData["error"] = value; }
        }
    }
}