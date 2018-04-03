using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Web.AuthorizeFilter;
using EzTask.Management.Infrastructures;
using EzTask.Management.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Management.Controllers
{
    [TypeFilter(typeof(EzTaskAuthorizeFilter))]
    public class UserProfileController : EzTaskController
    {
        public UserProfileController(IServiceProvider serviceProvider, IHttpContextAccessor httpContext) :
            base(serviceProvider, httpContext)
        {
        }

        [Route("profile.html")]
        public async Task<IActionResult> Index()
        {
            PageTitle = "Profile";
            var profileData = await GetAccountInfo();
            return View(profileData);
        }

        [HttpPost]
        [Route("profile.html")]
        public async Task<IActionResult> UpdateInfo(AccountInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await EzTask.Account.UpdateAccount(model.MapToEntity());
            }
            ActiveTab = "setting";
            return View("Index");
        }

        /// <summary>
        /// Get account info
        /// </summary>
        /// <returns></returns>
        private async Task<AccountInfoModel> GetAccountInfo()
        {
            var data = await EzTask.Account.GetAccountInfo(AccountId);
            if (data == null)
                return new AccountInfoModel();
            return data.MapToModel();
        }
    }
}