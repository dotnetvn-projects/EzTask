using EzTask.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Web.ExceptionFilter
{
    public class EzTaskExceptionFilter : ExceptionFilterAttribute
    {
        public ILogger Logger { get; set; }
        public override void OnException(ExceptionContext filterContext)
        {
           // Logger.LogError()
        }
    }
}
