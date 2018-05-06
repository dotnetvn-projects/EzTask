using EzTask.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Web.Filters
{
    public class ExceptionHandle : ExceptionFilterAttribute
    {
        public ILogger Logger { get; set; }
        public override void OnException(ExceptionContext filterContext)
        {
           // Logger.LogError()
        }
    }
}
