using AutoMapper;
using EzTask.Business;
using EzTask.Entity.Framework;
using EzTask.Framework;
using EzTask.Framework.Values;
using EzTask.Framework.Web.HttpContext;
using EzTask.Modules.Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EzTask.Modules.Core.Controllers
{
    public class EzTaskController: Controller
    {
        protected SessionManager SessionManager;
        protected CookiesManager CookiesManager;
        protected EzTaskBusiness EzTask;

        public EzTaskController(IServiceProvider serviceProvider)
        {
            InvokeComponents(serviceProvider);
        }

        /// <summary>
        /// Account Id
        /// </summary>
        protected int AccountId
        {
            get { return int.Parse(CurrentAccount.AccountId); }
        }

        /// <summary>
        /// Page title
        /// </summary>
        protected string PageTitle1
        {
            get { return ViewData["Title"]?.ToString(); }
            set { ViewData["Title"] = "EzTask - " + value; }
        }

        /// <summary>
        /// Error Message
        /// </summary>
        protected string ErrorMessage
        {
            get { return TempData["error"]?.ToString(); }
            set { TempData["error"] = value; }
        }

        /// <summary>
        /// Success Message
        /// </summary>
        protected string SuccessMessage
        {
            get { return TempData["success"]?.ToString(); }
            set { TempData["success"] = value; }
        }

        /// <summary>
        /// Current Login Account
        /// </summary>
        protected CurrentAccount CurrentAccount
        {
            get
            {
                var currentAccount = SessionManager.GetObject<CurrentAccount>(Key.Account);
                if (currentAccount == null)
                    currentAccount = new CurrentAccount();
                return currentAccount;
            }
            set
            {
                SessionManager.SetObject(Key.Account, value);
            }
        }

        /// <summary>
        /// Suspend session
        /// </summary>
        protected void SuspendSession(Key key)
        {
            SessionManager.Remove(key);
        }

        /// <summary>
        /// Suspend cookie
        /// </summary>
        protected void SuspendCookie(Key key)
        {
            CookiesManager.Remove(key);
        }

        protected void RememberMe()
        {
            CookiesManager.SetObject(Key.RememberMe, CurrentAccount, 3000);
        }

        #region Private
        private void InvokeComponents(IServiceProvider serviceProvider)
        {
           // HttpContext.RequestServices.InvokeComponents(out EzTask);
            serviceProvider.InvokeComponents(out EzTask);
            EzTaskMapper.Config(serviceProvider.InvokeComponents<IMapper>());
            serviceProvider.InvokeComponents(out SessionManager);
            serviceProvider.InvokeComponents(out CookiesManager);
        }
        #endregion
    }
}
