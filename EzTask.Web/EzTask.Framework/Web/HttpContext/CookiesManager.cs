using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Web.HttpContext
{
    public class CookiesManager
    {
        private EzTaskHttpContext _ezTaskHttp;
        public CookiesManager(IHttpContextAccessor httpContext)
        {
            _ezTaskHttp = new EzTaskHttpContext(httpContext);
        }
    }
}
