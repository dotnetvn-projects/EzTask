﻿using EzTask.Business;
using EzTask.Framework.GlobalData;
using EzTask.Web.Framework.WebContext;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EzTask.Modules.Core.Controllers
{
    public class BaseController : Controller
    {
        protected SessionManager SessionManager;
        protected EzTaskBusiness EzTask;

        public BaseController(IServiceProvider serviceProvider)
        {
            InvokeComponents(serviceProvider);
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

        protected void ResetAuthInfo()
        {
            Context.SuspendSession(SessionKey.Account);
            Context.SuspendCookie(SessionKey.EzTaskAuthen);
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        #region Private
        private void InvokeComponents(IServiceProvider serviceProvider)
        {
            serviceProvider.InvokeComponents(out EzTask);
            serviceProvider.InvokeComponents(out SessionManager);
        }
        #endregion
    }
}
