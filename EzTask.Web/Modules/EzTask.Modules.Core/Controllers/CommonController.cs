using System;
using System.IO;
using System.Threading.Tasks;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.HttpContext;
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
                accountId = Context.CurrentAccount.AccountId;

            var dataImage = await EzTask.Account.LoadAvatar(accountId);
            if (dataImage == null || dataImage.Length == 0)
            {
                string noAvatar = Path.Combine(hostingEnvironment.WebRootPath, "images/no-avatar.jpg");
                dataImage = await IO.ReadFile(noAvatar);
            }
            return File(dataImage, "image/jpeg");
        }

        /// <summary>
        /// Get new notify items
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("notify/new-list.html")]
        public IActionResult GetNewNotifyList()
        {
            return ViewComponent("NewNotificationList", new { Context.CurrentAccount.AccountId });
        }

        /// <summary>
        /// Get task notify items
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("notify/task-list.html")]
        public IActionResult GetTaskNotifyList()
        {
            return ViewComponent("TaskNotificationList", new { assigneeId = Context.CurrentAccount.AccountId });
        }

        [Route("not-found.html")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("error.html")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
