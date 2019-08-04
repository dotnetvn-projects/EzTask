using EzTask.Framework.Data;
using EzTask.Web.Framework.Infrastructures;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    throw new HttpException(context.Response.StatusCode);
                }
            }
            catch (HttpException ex)
            {
                await HandleHttpRequestAsync(context);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //TODO add log
            context.Items["originalPath"] = context.Request.Path.Value;
            context.Request.Path = "/error.html";
            await next(context);
        }

        private async Task HandleHttpRequestAsync(HttpContext context)
        {
            //TODO add log
            string originalPath = context.Request.Path.Value;

            if (context.Response.StatusCode == 404)
            {
                context.Items["originalPath"] = originalPath;
                if(Context.CurrentAccount.AccountId == 0)
                {
                    context.Request.Path = "/error/not-found.html";
                }
                else
                {
                    context.Request.Path = "/not-found.html";
                }
            }       

            await next(context);
        }
    }
}
