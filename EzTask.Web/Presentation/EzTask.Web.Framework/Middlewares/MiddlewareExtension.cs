using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.Middlewares
{
    public static class MiddlewareExtension
    {
        public static void UseHttpProcessMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<HttpRequestMiddleware>();
        }
    }
}
