using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Framework.Data;
using EzTask.Model.Enum;
using EzTask.Modules.Authentication.ViewModels;
using EzTask.Modules.Core.Controllers;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Authentication.Controllers
{
    public class RecoveryController : BaseController
    {
        public RecoveryController(IServiceProvider serviceProvider) :
              base(serviceProvider)
        {
        }

        #region Recover password
        [Route("auth/recover-password.html")]
        public IActionResult RecoverPassword()
        {
            return View(new RecoverPasswordViewModel());
        }

        [Route("auth/recover-password.html")]
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel viewModel)
        {
            var result = await EzTask.Account.CreateRecoverSession(viewModel.Email);
            if (result.Status == ActionStatus.Ok)
            {
                var emailTitle = Context.GetStringResource("RecoverTitleEmail", StringResourceType.AuthenticationPage);
                EzTask.Account.SendRecoverLink(viewModel.Email, result.Data.Account.DisplayName, emailTitle, result.Data.Id.ToString());

                SuccessMessage = string.Format(Context.GetStringResource("SendRecoverSuccess", StringResourceType.AuthenticationPage), viewModel.Email);
            }
            else
            {
                ErrorMessage = string.Format(Context.GetStringResource("SendRecoverFailed", StringResourceType.AuthenticationPage), viewModel.Email);
            }

            return View(viewModel);
        }

        [Route("auth/recover-password.html")]
        public IActionResult RecoverPasswordAction(string code)
        {
            return View(new RecoverPasswordViewModel());
        }

        [Route("auth/recover-password.html")]
        [HttpPost]
        public IActionResult RecoverPasswordAction(RecoverPasswordViewModel viewModel)
        {
            return View(viewModel);
        }

        [Route("auth/recover-success.html")]
        public IActionResult RecoverPasswordSuccess(string code)
        {
            return View();
        }

        #endregion
    }
}