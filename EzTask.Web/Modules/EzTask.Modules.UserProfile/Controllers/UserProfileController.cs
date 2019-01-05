using System;
using System.Threading.Tasks;
using EzTask.Models;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.UserProfile.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class UserProfileController : BaseController
    {
        public UserProfileController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        /// <summary>
        /// Public profile for member
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        [Route("public-profile.html")]
        [PageTitle("Profile - ")]
        public async Task<IActionResult> PublicProfile(string account)
        {
            PageTitleAttribute.CombineWith(this, account);
            var profileData = await GetAccountInfo();
            return View(profileData);
        }

        /// <summary>
        /// Profile for current logined user
        /// </summary>
        /// <returns></returns>
        [Route("profile.html")]
        [PageTitle("Profile")]
        public async Task<IActionResult> Profile()
        {
            var profileData = await GetAccountInfo();
            return View(profileData);
        }

        /// <summary>
        /// Update account info for current logined user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("profile.html")]
        public async Task<IActionResult> UpdateInfo(AccountInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await EzTask.Account.UpdateAccount(model);
                await EzTask.Skill.SaveAccountSkill(model.Skills, Context.CurrentAccount.AccountId);
            }
            return View("Profile", model);
        }

        /// <summary>
        /// Update avatar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-avatar.html")]
        public async Task<IActionResult> UpdateAvatar(IFormFile file)
        {
            if (file.Length > 0)
            {
                var stream = file.OpenReadStream();
                var result = await EzTask.Account.UpdateAvatar(Context.CurrentAccount.AccountId, stream);
                return Ok();
            }
            return BadRequest();
        }

        #region Non-Action
        /// <summary>
        /// Get account info
        /// </summary>
        /// <returns></returns>
        private async Task<AccountInfoModel> GetAccountInfo()
        {
            var data = await EzTask.Account.GetAccountInfo(Context.CurrentAccount.AccountId);            
            if(data != null)
            {
                data.Skills = await EzTask.Skill.GetSkill(Context.CurrentAccount.AccountId);
            }
            return data;
        }
        #endregion
    }
}