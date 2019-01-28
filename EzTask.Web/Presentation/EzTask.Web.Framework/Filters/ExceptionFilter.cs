using EzTask.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.Filters
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
