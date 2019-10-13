using EzTask.Interface;
using Microsoft.AspNetCore.Hosting;

namespace EzTask.Web.Framework.Infrastructures
{
    public class WebHost : IWebEnvironment
    {
        private readonly IWebHostEnvironment _hosting;
        public WebHost(IWebHostEnvironment host)
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
