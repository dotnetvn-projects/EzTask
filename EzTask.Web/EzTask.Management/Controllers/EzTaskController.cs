using System;
using EzTask.Framework.Enum;
using EzTask.Framework.Web.HttpContext;
using EzTask.Interfaces;
using EzTask.MainBusiness;
using EzTask.Management.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Management.Controllers
{
    public class EzTaskController : Controller
    {
        private SessionManager _sessionManager;
        protected static EzTaskBusiness EzTask { get; private set; }

        public EzTaskController(IServiceProvider serviceProvider,
            IHttpContextAccessor httpContext)
        {
            if (EzTask == null)
            {
                EzTask = serviceProvider.GetService<IEzTaskBusiness>() as EzTaskBusiness;
            }
            _sessionManager = new SessionManager(httpContext);
        }

        /// <summary>
        /// Page title
        /// </summary>
        protected string PageTitle
        {
            get { return "EzTask - "+ ViewData["Title"]?.ToString(); }
            set { ViewData["Title"] = value; }
        }

        /// <summary>
        /// ErrorMessage
        /// </summary>
        protected string ErrorMessage
        {
            get { return ViewData["error"]?.ToString(); }
            set { ViewData["error"] = value; }
        }

        protected AccountModel CurrentAccount
        {
            get
            {
                return _sessionManager.GetObject<AccountModel>(EzTaskKey.Account.ToString()) as AccountModel;
            }
            set
            {
                _sessionManager.SetObject(EzTaskKey.Account.ToString(), value);
            }
        }

        protected void SuspendAccountSession()
        {
            _sessionManager.Suspend(EzTaskKey.Account.ToString());
        }
    }
}