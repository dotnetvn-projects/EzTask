using System;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using System.Threading.Tasks;
using EzTask.Management.Models.Account;
using EzTask.Framework.Message;
using EzTask.Entity.Framework;
using Microsoft.AspNetCore.Authorization;

namespace EzTask.Management.Controllers
{  
    public class AccountController : EzTaskController
    {
        public AccountController(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        #region Manual Login

        /// <summary>
        /// Login view
        /// </summary>
        /// <returns></returns>
        [Route("login.html")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            PageTitle = "Login";
            return View(new LoginModel());
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login.html")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var account = await EzTask.Account.GetAccount(model.AccountName, model.Password);

                if (account != null)
                {
                    if (account.AccountStatus != (int)AccountStatus.Block)
                    {
                        CurrentAccount = CurrentAccount.Create(account.Id.ToString(),
                            account.AccountName,
                            account.AccountInfo.DisplayName,
                            account.AccountInfo.JobTitle,
                            account.CreatedDate);

                        return RedirectToAction("Index", "Home");
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            PageTitle = "Register new membership";
            return View(new RegisterModel());
        }

        /// <summary>
        /// Register action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register.html")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var account = await EzTask.Account.RegisterNew(model.MapToEntity());

                if (account != null)
                {
                    return RedirectToAction("Login", "Account");                   
                }

                ErrorMessage = AccountMessage.CreateFailed;
            }
            catch
            {
                ErrorMessage = AccountMessage.LoginFailed;               
            }

            return View();
        }

        #endregion

        #region Login use social network
        #endregion

        [HttpPost]
        public IActionResult LogOff()
        {
            SuspendAccountSession();
            return RedirectToAction("Index", "Home");
        }
    }
}