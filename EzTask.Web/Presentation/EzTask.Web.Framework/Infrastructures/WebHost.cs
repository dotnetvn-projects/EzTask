using EzTask.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.Infrastructures
{
    public class WebHost : Interfaces.IWebHostEnvironment
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
