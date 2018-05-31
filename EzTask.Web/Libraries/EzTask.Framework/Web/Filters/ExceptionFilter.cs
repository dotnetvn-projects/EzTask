using EzTask.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Framework.Web.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public ILogger Logger { get; set; }      

        public Task OnExceptionAsync(ExceptionContext context)
        {

                throw new NotImplementedException();

        }
    }
}
