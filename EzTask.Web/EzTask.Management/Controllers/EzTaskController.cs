using System;
using EzTask.Entity.Framework;
using EzTask.Framework.Values;
using EzTask.Framework.Web.HttpContext;
using EzTask.Interfaces;
using EzTask.MainBusiness;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Management.Controllers
{
    public class EzTaskController : Controller
    {
        protected SessionManager _sessionManager;
        protected EzTaskBusiness EzTask { get; private set; }
        protected IHttpContextAccessor HttpContextAccessor;

        public EzTaskController(IServiceProvider serviceProvider)
        {
            EzTask = serviceProvider.GetService<IEzTaskBusiness>() as EzTaskBusiness;          
           _sessionManager = new SessionManager();
            PageTitle = string.Empty;
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
        protected string PageTitle
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

        protected string ActiveTab
        {
            get { return ViewData["activeTab"]?.ToString(); }
            set { ViewData["activeTab"] = value; }
        }

        /// <summary>
        /// Current Login Account
        /// </summary>
        protected CurrentAccount CurrentAccount
        {
            get
            {
                var currentAccount = _sessionManager.GetObject<CurrentAccount>(EzTaskKey.Account);
                if (currentAccount == null)
                    currentAccount = new CurrentAccount();
                return currentAccount;
            }
            set
            {
                _sessionManager.SetObject(EzTaskKey.Account, value);
            }
        }

        public SessionManager SessionManager { get => _sessionManager; set => _sessionManager = value; }    

        /// <summary>
        /// Suspend session for current login account
        /// </summary>
        protected void SuspendAccountSession()
        {
            _sessionManager.Suspend(EzTaskKey.Account);
        }
    }
}