using System;
using EzTask.Management.Models;
using Microsoft.AspNetCore.Mvc;
using EzTask.Management.Infrastructures;
using System.Threading.Tasks;

namespace EzTask.Management.Controllers
{  
    public class AccountController : EzTaskController
    {
        public AccountController(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        [Route("login.html")]
        public IActionResult Login()
        {
            return View(new AccountModel());
        }

        [HttpPost]
        [Route("login.html")]
        public async Task<IActionResult> Login(AccountModel model)
        {
            if (!ModelState.IsValid)
                return View();
                      
           var account = await EzTask.Account.GetAccount(model.AccountName, model.Password);

            if(account == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult LogOff()
        {  
            return RedirectToAction("Index", "Home");
        }
    }
}