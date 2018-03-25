using System;
using EzTask.Management.Models;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using System.Threading.Tasks;
using EzTask.Management.Models.Account;

namespace EzTask.Management.Controllers
{  
    public class AccountController : EzTaskController
    {
        public AccountController(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        #region Login

        [Route("login.html")]
        public IActionResult Login()
        {
            PageTitle = "Login";
            return View(new LoginModel());
        }

        [HttpPost]
        [Route("login.html")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var account = await EzTask.Account.GetAccount(model.AccountName,
               model.Password);

            if (account == null)
            {
                ErrorMessage = "Sorry we cannot create for your account";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register New Account

        [Route("register.html")]
        public IActionResult Register()
        {
            PageTitle = "Register new membership";
            return View(new RegisterModel());
        }

        [HttpPost]
        [Route("register.html")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var account = await EzTask.Account.RegisterNew(model.MapToEntity());

            if (account == null)
            {
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        #endregion

        [HttpPost]
        public IActionResult LogOff()
        {  
            return RedirectToAction("Index", "Home");
        }

    }
}