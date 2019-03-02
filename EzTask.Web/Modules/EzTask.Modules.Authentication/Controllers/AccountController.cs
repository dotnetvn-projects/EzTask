using System;
using System.Threading.Tasks;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using EzTask.Model.Enum;
using EzTask.Web.Framework.WebContext;

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
            return View(new LoginModel { RedirectUrl = redirect });
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login.html")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var account = await EzTask.Account.Login(model);

                if (account != null)
                {
                    if (account.AccountStatus != AccountStatus.Block)
                    {
                        var currentAccount = CurrentAccount.Create(account.AccountId,
                            account.AccountName, account.DisplayName,
                            account.JobTitle, account.LangDisplay, account.CreatedDate);

                        Context.CurrentAccount.Set(currentAccount);

                        Context.RememberLogin(currentAccount);

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
            catch (Exception ex)
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
            return View(new RegisterModel());
        }

        /// <summary>
        /// Register action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register.html")]
        public async Task<IActionResult> Register(RegisterModel model)
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
                            var account = await EzTask.Account.RegisterNew(model);

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
            return RedirectToAction("Login", "Account");
        }
    }
}
