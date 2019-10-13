using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EzTask.Model;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using IO = EzTask.Framework.IO.FileIO;

namespace EzTask.Modules.Core.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    [TypeFilter(typeof(AuthenticationAttribute))]
    public class CommonController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public CommonController(IServiceProvider serviceProvider,
            IWebHostEnvironment environment) : base(serviceProvider)
        {
            hostingEnvironment = environment;
        }

        /// <summary>
        /// Load avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Not found for logined user
        /// </summary>
        /// <returns></returns>
        [Route("not-found.html")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Error for logined user
        /// </summary>
        /// <returns></returns>
        [Route("error.html")]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Not found for anonymous user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("error/not-found.html")]
        public IActionResult PageNotFoundAnonymous()
        {
            return View();
        }
    }
}
