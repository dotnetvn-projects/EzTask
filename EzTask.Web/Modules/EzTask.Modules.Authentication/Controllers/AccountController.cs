using System;
using System.Threading.Tasks;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using EzTask.Model.Enum;
using EzTask.Web.Framework.WebContext;
using EzTask.Modules.Authentication.ViewModels;

namespace EzTask.Modules.Authentication.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        #region Manual Login

        /// <summary>
        /// Login view
        /// </summary>
        /// <returns></returns>
        [Route("login.html")]
        public IActionResult Login(string redirect)
        {
            return View(new LoginViewModel { RedirectUrl = redirect });
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login.html")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var account = await EzTask.Account.Login(new AccountModel {
                    AccountName = model.AccountName,
                    Password = model.Password
                });

                if (account != null)
                {
                    if (account.AccountStatus != AccountStatus.Block)
                    {
                        var currentAccount = CurrentAccount.Create(account.AccountId,
                            account.AccountName, account.DisplayName,
                            account.JobTitle, account.LangDisplay, account.CreatedDate);

                        Context.CurrentAccount.Set(currentAccount);

                        if (model.RememberMe)
                        {
                            Context.RememberLogin(currentAccount);
                        }

                        Context.SetLanguageLocalization(account.LangDisplay);

                        if (string.IsNullOrEmpty(model.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(model.RedirectUrl);
                        }
                    }
                    else
                    {
                        ErrorMessage = Context.GetStringResource("AccountBlocked", StringResourceType.AuthenticationPage);
                    }
                }
                else
                {
                    ErrorMessage = Context.GetStringResource("LoginFailed", StringResourceType.AuthenticationPage);
                }
            }
            catch
            {
                ErrorMessage = Context.GetStringResource("LoginFailed", StringResourceType.AuthenticationPage);
            }

            return View();
        }

        #endregion

        #region Manual Register New Account

        /// <summary>
        /// Register view
        /// </summary>
        /// <returns></returns>
        [Route("register.html")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// Register action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register.html")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (model.Password != model.PasswordTemp)
                {
                    ErrorMessage = Context.GetStringResource("ConfirmPasswordNotMatch", StringResourceType.AuthenticationPage);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var existAccount = await EzTask.Account.GetAccount(model.AccountName);
                        if (existAccount == null)
                        {
                            model.DisplayName = model.FullName;
                            var account = await EzTask.Account.RegisterNew(new AccountModel {
                                AccountName = model.AccountName,
                                Password = model.Password,
                                FullName = model.FullName,
                                DisplayName = model.DisplayName
                            });

                            if (account != null)
                            {
                                return RedirectToAction("Login", "Account");
                            }

                            ErrorMessage = Context.GetStringResource("CreateAccountFailed", StringResourceType.AuthenticationPage);
                        }
                        else
                        {
                            ErrorMessage = Context.GetStringResource("AccountExisted", StringResourceType.AuthenticationPage);;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Context.GetStringResource("CreateAccountFailed", StringResourceType.AuthenticationPage);
            }

            return View();
        }

        #endregion

        #region Login use social network
        #endregion

        [Route("logout.html")]
        public IActionResult LogOff()
        {
            Context.SuspendSession(SessionKey.Account);
            Context.SuspendCookie(SessionKey.EzTaskAuthen);

            return RedirectToAction("Login", "Account");
        }

        #region Recover password
        [Route("recover-password.html")]
        public IActionResult RecoverPassword()
        {
            return View(new RecoverPasswordViewModel());
        }

        [Route("recover-password.html")]
        [HttpPost]
        public IActionResult RecoverPassword(RecoverPasswordViewModel viewModel)
        {
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
        #endregion
    }
}
