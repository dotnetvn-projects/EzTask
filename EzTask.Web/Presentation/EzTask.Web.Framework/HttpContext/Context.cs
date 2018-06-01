using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EzTask.Web.Framework.HttpContext
{
    public static class Context
    {
        private static IHttpContextAccessor _httpContextAccessor;
    
        public static void Configure(this IApplicationBuilder applicationBuilder,
            IHttpContextAccessor httpContextAccessor)
        {
                _httpContextAccessor = httpContextAccessor;
        }

        public static string GetRemoteIP
        {
            get { return Current.Connection.RemoteIpAddress.ToString(); }
        }

        public static string GetUserAgent
        {
            get { return Current.Request.Headers["User-Agent"].ToString(); }
        }

        public static string GetScheme
        {
            get { return Current.Request.Scheme; }
        }

        public static string GetHost
        {
            get { return Current.Request.Host.ToUriComponent(); }
        }

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get => _httpContextAccessor.HttpContext;
        }
    }
}
