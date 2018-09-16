using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Models;
using EzTask.Web.Framework.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using IO = EzTask.Framework.IO.FileIO;
namespace EzTask.Modules.Core.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class CommonController : BaseController
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public CommonController(IServiceProvider serviceProvider,
            IHostingEnvironment environment) : base(serviceProvider)
        {
            hostingEnvironment = environment;
        }

        [Route("profile-avatar.html")]
        public async Task<IActionResult> LoadAvatar(int accountId = 0)
        {
            if (accountId == 0)
                accountId = AccountId;

            var dataImage = await EzTask.Account.LoadAvatar(AccountId);
            if (dataImage == null || dataImage.Length == 0)
            {
                string noAvatar = Path.Combine(hostingEnvironment.WebRootPath, "images/no-avatar.jpg");
                dataImage = await IO.ReadFile(noAvatar);
            }
            return File(dataImage, "image/jpeg");
        }

        [Route("not-found.html")]
        [PageTitle("Page not found")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
