using EzTask.Business;
using EzTask.Framework.Data;
using EzTask.Model;
using EzTask.Model.Message;
using EzTask.Web.Framework.HttpContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzTask.Modules.Core.Controllers
{
    public class BaseController: Controller
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

        /// <summary>
        /// Create response message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected ResponseMessage CreateResponseMessage(string title, string message)
        {
            return new ResponseMessage
            {
                Title = title,
                Message = message
            };
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
