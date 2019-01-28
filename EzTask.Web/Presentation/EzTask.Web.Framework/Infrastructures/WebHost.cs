using EzTask.Interface;
using Microsoft.AspNetCore.Hosting;

namespace EzTask.Web.Framework.Infrastructures
{
    public class WebHost : IWebHostEnvironment
    {
        private readonly IHostingEnvironment _hosting;
        public WebHost(IHostingEnvironment host)
        {
            _hosting = host;
        }

        public string GetRootContentUrl()
        {
            return _hosting.WebRootPath;
        }

        public string GetRootUrl()
        {
            return _hosting.ContentRootPath;
        }
    }
}
