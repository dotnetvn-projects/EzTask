using Microsoft.AspNetCore.Builder;

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
