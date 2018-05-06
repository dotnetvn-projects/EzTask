using System;
using System.IO;
using System.Threading.Tasks;
using EzTask.Framework.Web.Attributes;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Core.Infrastructures;
using EzTask.Modules.Core.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.UserProfile.Controllers
{
    [TypeFilter(typeof(AuthenAttribute))]
    public class UserProfileController : EzTaskController
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
        [Route("pubic-profile.html")]
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
                var account = await EzTask.Account.UpdateAccount(model.MapToEntity());
                await EzTask.Skill.SaveAccountSkill(model.Skills, AccountId);
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
                var result = await EzTask.Account.UpdateAvatar(AccountId, stream);
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
            var data = await EzTask.Account.GetAccountInfo(AccountId);            
            var model = data.MapToModel();
            if(model != null)
            {
                model.Skills = await EzTask.Skill.GetSkill(AccountId);
            }
            return model;
        }
        #endregion
    }
}