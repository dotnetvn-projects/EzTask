using EzTask.Business;
using EzTask.Framework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.ViewComponents
{
    public class EzTaskViewComponent:ViewComponent
    {
        protected EzTaskBusiness EzTask;

        public EzTaskViewComponent(IServiceProvider serviceProvider)
        {
            InvokeComponents(serviceProvider);
        }

        #region Private
        private void InvokeComponents(IServiceProvider serviceProvider)
        {
            serviceProvider.InvokeComponents(out EzTask);
        }
        #endregion
    }
}
