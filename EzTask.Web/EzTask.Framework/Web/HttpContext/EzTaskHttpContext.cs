using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Web.HttpContext
{
    public class EzTaskHttpContext
    {
        private IHttpContextAccessor _httpContextAccessor;

        public EzTaskHttpContext(IHttpContextAccessor httpContextAccessor)
        {
            Context = httpContextAccessor;         
        }

        public IHttpContextAccessor Context
        {
            get => _httpContextAccessor;
            private set => _httpContextAccessor = value;
        }
    }
}
