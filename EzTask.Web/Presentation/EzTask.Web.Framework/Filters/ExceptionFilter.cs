using EzTask.Web.Framework.Infrastructures;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {

        public Task OnExceptionAsync(ExceptionContext context)
        {
            throw new HttpException(400, context.Exception.Message, context.Exception);
        }
    }
}
