using EzTask.Business;
using EzTask.Framework.Data;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EzTask.Modules.Core.Controllers
{
    public class CoreController: Controller
    {
        protected SessionManager SessionManager;
        protected CookiesManager CookiesManager;
        protected EzTaskBusiness EzTask;

        public CoreController(IServiceProvider serviceProvider)
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
                var currentAccount = SessionManager.GetObject<CurrentAccount>(AppKey.Account);
                if (currentAccount == null)
                    currentAccount = new CurrentAccount();
                return currentAccount;
            }
            set
            {
                SessionManager.SetObject(AppKey.Account, value);
            }
        }

        /// <summary>
        /// Suspend session
        /// </summary>
        protected void SuspendSession(AppKey key)
        {
            SessionManager.Remove(key);
        }

        /// <summary>
        /// Suspend cookie
        /// </summary>
        protected void SuspendCookie(AppKey key)
        {
            CookiesManager.Remove(key);
        }

        protected void RememberMe()
        {
            CookiesManager.SetObject(AppKey.EzTaskAuthen, CurrentAccount, 3000);
        }

        #region Private
        private void InvokeComponents(IServiceProvider serviceProvider)
        {
            serviceProvider.InvokeComponents(out EzTask);
            serviceProvider.InvokeComponents(out SessionManager);
            serviceProvider.InvokeComponents(out CookiesManager);
        }
        #endregion
    }
}
