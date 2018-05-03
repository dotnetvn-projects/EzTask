using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Entity.Framework;
using EzTask.Framework.Message;
using EzTask.Framework.Values;
using EzTask.Framework.Web.Filters;
using EzTask.Modules.Core.Controllers;
using EzTask.Modules.Core.Infrastructures;
using EzTask.Modules.Core.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace EzTask.Modules.Authentication.Controllers
{
    public class AccountController : EzTaskController
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

                var account = await EzTask.Account.Login(model.AccountName, model.Password);

                if (account != null)
                {
                    if (account.AccountStatus != (int)AccountStatus.Block)
                    {
                        CurrentAccount = CurrentAccount.Create(account.Id.ToString(), account.AccountName,
                            account.AccountInfo.DisplayName, account.AccountInfo.JobTitle, account.CreatedDate);

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
                            var account = await EzTask.Account.RegisterNew(model.MapToEntity());

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
            SuspendSession(Key.Account);
            return RedirectToAction("Login", "Account");
        }
    }
}
