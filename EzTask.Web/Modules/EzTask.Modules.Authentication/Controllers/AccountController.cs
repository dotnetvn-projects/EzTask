using System;
using System.Threading.Tasks;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using EzTask.Model.Enum;
using EzTask.Web.Framework.WebContext;
using EzTask.Modules.Authentication.ViewModels;
using EzTask.Web.Framework.Attributes;

namespace EzTask.Modules.Authentication.Controllers
{
    [TypeFilter(typeof(ApplyLanguageAttribute))]
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
            if(Context.CurrentAccount.IsLogined)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel { RedirectUrl = redirect });
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login.html")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var account = await EzTask.Account.Login(new AccountModel
            {
                AccountName = viewModel.AccountName,
                Password = viewModel.Password
            });

            ErrorMessage = Context.GetStringResource("LoginFailed", StringResourceType.AuthenticationPage);

            if (account != null)
            {
                if (account.AccountStatus != AccountStatus.Block)
                {
                    var currentAccount = CurrentAccount.Create(account.AccountId,
                        account.AccountName, account.DisplayName,
                        account.JobTitle, account.LangDisplay, account.CreatedDate);

                    Context.CurrentAccount.Set(currentAccount);

                    if (viewModel.RememberMe)
                    {
                        Context.RememberLogin(currentAccount);
                    }

                    Context.SetLanguageLocalization(account.LangDisplay);

                    if (string.IsNullOrEmpty(viewModel.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(viewModel.RedirectUrl);
                    }
                }
                else
                {
                    ErrorMessage = Context.GetStringResource("AccountBlocked", StringResourceType.AuthenticationPage);
                }
            }

            return View(viewModel);
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
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {           
            if (viewModel.Password != viewModel.PasswordTemp)
            {
                ErrorMessage = Context.GetStringResource("ConfirmPasswordNotMatch", StringResourceType.AuthenticationPage);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var existAccount = await EzTask.Account.GetAccount(viewModel.AccountName);
                    if (existAccount == null)
                    {
                        viewModel.DisplayName = viewModel.FullName;

                        var account = await EzTask.Account.RegisterNew(new AccountModel
                        {
                            AccountName = viewModel.AccountName,
                            Password = viewModel.Password,
                            FullName = viewModel.FullName,
                            DisplayName = viewModel.DisplayName
                        });

                        if (account != null)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            ErrorMessage = Context.GetStringResource("CreateAccountFailed", StringResourceType.AuthenticationPage);
                        }
                    }
                    else
                    {
                        ErrorMessage = Context.GetStringResource("AccountExisted", StringResourceType.AuthenticationPage);
                    }
                }
            }

            return View(viewModel);
        }

        #endregion

        [Route("logout.html")]
        public IActionResult LogOff()
        {
            Context.SuspendSession(SessionKey.Account);
            Context.SuspendCookie(SessionKey.EzTaskAuthen);

            return RedirectToAction("Login", "Account");
        }
    }
}
