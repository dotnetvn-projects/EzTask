using EzTask.Framework.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
                await HandleHttpRequestAsync(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
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

            if (context.Response.StatusCode == 404
                && !string.IsNullOrEmpty(context.Session.GetString(SessionKey.Account.ToString())))
            {
                context.Items["originalPath"] = originalPath;
                context.Request.Path = "/not-found.html";
            }
            else if (context.Response.StatusCode == 404)
            {
                context.Items["originalPath"] = originalPath;
                context.Request.Path = "/error/not-found.html";
            }

            await next(context);
        }
    }
}
