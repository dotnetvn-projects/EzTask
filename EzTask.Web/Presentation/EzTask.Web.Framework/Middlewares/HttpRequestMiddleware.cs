using EzTask.Framework.GlobalData;
using EzTask.Interface;
using EzTask.Log;
using EzTask.Web.Framework.Infrastructures;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace EzTask.Web.Framework.Middlewares
{
    public class HttpRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public HttpRequestMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                LogRequest(context);

                await _next(context);

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
            LogError(ex);

            if (context.Request.IsAjaxRequest())
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(Context.GetStringResource("RequestFailed", StringResourceType.Error));
            }
            else
            {
                context.Items["originalPath"] = context.Request.Path.Value;
                context.Request.Path = "/error.html";
                await _next(context);
            }
        }

        private async Task HandleHttpRequestAsync(HttpContext context)
        {
            string originalPath = context.Request.Path.Value;

            LogPageNotFound(context);

            if (context.Response.StatusCode == 404)
            {
                context.Items["originalPath"] = originalPath;
                if (Context.CurrentAccount.AccountId == 0)
                {
                    context.Request.Path = "/error/not-found.html";
                }
                else
                {
                    context.Request.Path = "/not-found.html";
                }
            }

            await _next(context);
        }

        private void LogRequest(HttpContext context)
        {
            if (context.Request.IsAjaxRequest())
            {
                _logger.LogEntity = LogEntity.Create(Context.CurrentAccount.AccountName,
                     "Request Ajax Url: " + context.Request.Path.Value, "App");
            }
            else
            {
                _logger.LogEntity = LogEntity.Create(Context.CurrentAccount.AccountName,
                     "Request Url: " + context.Request.Path.Value, "App");
            }


            _logger.WriteInfo();
        }

        private void LogError(Exception ex)
        {
            _logger.LogEntity = LogEntity.Create(Context.CurrentAccount.AccountName,
                    ex.Message, ex.TargetSite.Name, ex);

            _logger.WriteError();
        }

        private void LogPageNotFound(HttpContext context)
        {
            _logger.LogEntity = LogEntity.Create(Context.CurrentAccount.AccountName,
                    "Page not found: " + context.Request.Path.Value, "App");

            _logger.WriteError();
        }
    }
}
