using System;
using System.Threading.Tasks;
using EzTask.Framework.Message;
using EzTask.Framework.Data;
using EzTask.Models;
using EzTask.Modules.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using EzTask.Web.Framework.Attributes;
using EzTask.Models.Enum;

namespace EzTask.Modules.Authentication.Controllers
{
    public class AccountController : CoreController
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
        [PageTitle("Login")]
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
                        CurrentAccount = CurrentAccount.Create(account.AccountId.ToString(), 
                            account.AccountName, account.DisplayName, 
                            account.JobTitle, account.CreatedDate);

                        RememberMe();

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
                        ErrorMessage = AccountMessage.AccountBlock;
                    }
                }
                else
                {
                    ErrorMessage = AccountMessage.LoginFailed;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = AccountMessage.LoginFailed;
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
        [PageTitle("Register new membership")]
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
                    ErrorMessage = AccountMessage.ConfirmPasswordNotMatch;
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

                            ErrorMessage = AccountMessage.CreateFailed;
                        }
                        else
                        {
                            ErrorMessage = AccountMessage.ExistAccount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = AccountMessage.CreateFailed;
            }

            return View();
        }

        #endregion

        #region Login use social network
        #endregion

        [HttpPost]
        public IActionResult LogOff()
        {
            SuspendSession(AppKey.Account);
            return RedirectToAction("Login", "Account");
        }
    }
}
