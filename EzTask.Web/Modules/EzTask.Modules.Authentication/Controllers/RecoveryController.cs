using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Common;
using EzTask.Framework.GlobalData;
using EzTask.Model.Enum;
using EzTask.Modules.Authentication.ViewModels;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.Attributes;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Authentication.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
    public class RecoveryController : BaseController
    {
        public RecoveryController(IServiceProvider serviceProvider) :
              base(serviceProvider)
        {
        }

        #region Recover password

        /// <summary>
        /// Recovery password view
        /// </summary>
        /// <returns></returns>
        [Route("recover-password.html")]
        public IActionResult RecoverPassword()
        {
            return View(new RecoverPasswordViewModel
            {
                Password = Guid.NewGuid().ToString(),
                ConfirmPassword = Guid.NewGuid().ToString()
            });
        }

        /// <summary>
        /// Recovery password action
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Route("recover-password.html")]
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await EzTask.Account.CreateRecoverSession(viewModel.Email);
                if (result.Status == ActionStatus.Ok)
                {
                    var emailTitle = Context.GetStringResource("RecoverTitleEmail", StringResourceType.AuthenticationPage);
                    EzTask.Account.SendRecoverLink(viewModel.Email, result.Data.Account.DisplayName, emailTitle, result.Data.Code);

                    SuccessMessage = string.Format(Context.GetStringResource("SendRecoverSuccess", StringResourceType.AuthenticationPage), viewModel.Email);
                    viewModel.IsSuccess = true;
                }
                else
                {
                    ErrorMessage = string.Format(Context.GetStringResource("SendRecoverFailed", StringResourceType.AuthenticationPage), viewModel.Email);
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// Change password view
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("change-password.html")]
        public async Task<IActionResult> RecoverPasswordAction(string code)
        {
            var viewModel = new RecoverPasswordViewModel();
            var sessionData = await EzTask.Account.GetRecoverSession(code);

            if (sessionData.Status != ActionStatus.Ok
                || sessionData.Data.ExpiredTime < DateTime.Now)
            {
                ErrorMessage = Context.GetStringResource("RecoverSessionExpired", StringResourceType.AuthenticationPage);
                viewModel.IsExpired = true;
            }

           
            if (sessionData.Status == ActionStatus.Ok)
            {
                viewModel.AccountName = sessionData.Data.Account.AccountName;
                viewModel.RecoverCode = code;
                viewModel.Email = StringUtilities.CreateFakeEmail();
            }
            return View(viewModel);
        }

        /// <summary>
        /// Change password action
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Route("change-password.html")]
        [HttpPost]
        public async Task<IActionResult> RecoverPasswordAction(RecoverPasswordViewModel viewModel)
        {
            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                ErrorMessage = Context.GetStringResource("ConfirmPasswordNotMatch", StringResourceType.AuthenticationPage);
            }
            else if (ModelState.IsValid)
            {
                var result = await EzTask.Account.UpdatePassword(viewModel.AccountName, viewModel.Password, viewModel.Password, true);

                if (result.Status == ActionStatus.Ok)
                {
                    await EzTask.Account.DeleteRecoverSession(viewModel.RecoverCode);

                    return RedirectToAction("RecoverPasswordSuccess", new { code = viewModel.RecoverCode });
                }
                else
                {
                    ErrorMessage = Context.GetStringResource("RecoverPasswordFailed", StringResourceType.AuthenticationPage);
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// Recovery sucess view
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("recover-success.html")]
        public IActionResult RecoverPasswordSuccess(string code)
        {
            return View();
        }

        #endregion
    }
}