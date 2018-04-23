using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using IO = EzTask.Framework.IO.File;

namespace EzTask.Web.Controllers
{
    [TypeFilter(typeof(AuthorizeFilter))]
    public class CommonController : EzTaskController
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public CommonController(IServiceProvider serviceProvider, 
            IHostingEnvironment environment) : base(serviceProvider)
        {
            hostingEnvironment = environment;
        }

        [Route("profile-avatar.html")]
        public async Task<IActionResult> LoadAvatar()
        {
            var dataImage = await EzTask.Account.LoadAvatar(AccountId);
            if(dataImage == null || dataImage.Length == 0)
            {         
                string no_image = Path.Combine(hostingEnvironment.WebRootPath, "images/no-avatar.jpg");
                dataImage = await IO.ReadFile(no_image);
            }
            return File(dataImage, "image/jpeg");
        }
    }
}