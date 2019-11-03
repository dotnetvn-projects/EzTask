using System;
using System.Threading.Tasks;
using EzTask.Framework.Common;
using EzTask.Framework.GlobalData;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Model.Message;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.UserProfile.ViewModels;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.UserProfile.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    [TypeFilter(typeof(AuthenticationAttribute))]
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
        public async Task<IActionResult> PublicProfile(string account)
        {
            var profileData = await GetAccountInfo();
            AccountInfoViewModel viewModel = ConvertToViewModel(profileData);
            return View(viewModel);
        }

        /// <summary>
        /// Profile for current logined user
        /// </summary>
        /// <returns></returns>
        [Route("profile.html")]
        public async Task<IActionResult> Profile()
        {
            var profileData = await GetAccountInfo();
            AccountInfoViewModel viewModel = ConvertToViewModel(profileData);
            return View(viewModel);
        }

        /// <summary>
        /// Update account info for current logined user
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-info.html")]
        public async Task<IActionResult> UpdateInfo(AccountInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await EzTask.Account.UpdateAccount(ConvertToModel(viewModel));
                await EzTask.Skill.SaveAccountSkill(viewModel.Skills, Context.CurrentAccount.AccountId);
            }
            return View("Profile", viewModel);
        }

        /// <summary>
        /// Update account info for current logined user
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-password.html")]
        public async Task<IActionResult> UpdatePassword(ChangePasswordViewModel viewModel)
        {
            ResponseMessage response = new ResponseMessage();

            if (ModelState.IsValid)
            {
                if (viewModel.NewPassword != viewModel.ConfirmNewPassword)
                {
                    response.Message = Context.GetStringResource("ConfirmPasswordNotMatch", StringResourceType.UserProfilePage);
                    return BadRequest(response);
                }

                var result = await EzTask.Account.UpdatePassword(Context.CurrentAccount.AccountName, 
                    viewModel.CurrentPassword, viewModel.NewPassword);                    

                if(result.Status == ActionStatus.NotFound)
                {
                    response.Message = Context.GetStringResource("PasswordNotMatch", StringResourceType.UserProfilePage);
                    return BadRequest(response);
                }
                else
                {

                    response.Message = Context.GetStringResource("UpdatePassSuccess", StringResourceType.UserProfilePage);
                    return Ok(response);
                }
            }

            response.Message = Context.GetStringResource("RequestFailed", StringResourceType.Error);
            return BadRequest(response);
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
                await EzTask.Account.UpdateAvatar(Context.CurrentAccount.AccountId, stream);
                return Ok();
            }
            return BadRequest(Context.GetStringResource("UploadAvatarError", StringResourceType.UserProfilePage));
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

        /// <summary>
        /// Convert model to viewmodel
        /// </summary>
        /// <param name="profileData"></param>
        /// <returns></returns>
        private static AccountInfoViewModel ConvertToViewModel(AccountInfoModel profileData)
        {
            return new AccountInfoViewModel
            {
                AccountInfoId = profileData.AccountInfoId,
                Address1 = profileData.Address1,
                Address2 = profileData.Address2,
                BirthDay = profileData.BirthDay.HasValue? profileData.BirthDay.Value.ToDateString() : string.Empty,
                Comment = profileData.Comment,
                Education = profileData.Education,
                Email = profileData.Email,
                Introduce = profileData.Introduce,
                IsPublished = profileData.AccountInfoId == 0 ? true : profileData.IsPublished,
                JobTitle = profileData.JobTitle,
                PhoneNumber = profileData.PhoneNumber,
                Skills = profileData.Skills,
                AccountId = profileData.AccountId,
                DisplayName = profileData.DisplayName,
                FullName = profileData.FullName
            };
        }

        /// <summary>
        /// Convert viewmodel to model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static AccountInfoModel ConvertToModel(AccountInfoViewModel viewModel)
        {
            return new AccountInfoModel
            {
                AccountInfoId = viewModel.AccountInfoId,
                Address1 = viewModel.Address1,
                Address2 = viewModel.Address2,
                BirthDay = DateTimeUtilities.ParseFromString(viewModel.BirthDay),
                Comment = viewModel.Comment,
                Education = viewModel.Education,
                Email = viewModel.Email,
                Introduce = viewModel.Introduce,
                IsPublished = viewModel.IsPublished,
                JobTitle = viewModel.JobTitle,
                PhoneNumber = viewModel.PhoneNumber,
                Skills = viewModel.Skills,
                AccountId = viewModel.AccountId,
                DisplayName = viewModel.DisplayName,
                FullName = viewModel.FullName
            };
        }
        #endregion
    }
}