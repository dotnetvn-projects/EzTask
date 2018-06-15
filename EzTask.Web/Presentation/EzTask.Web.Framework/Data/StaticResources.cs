using EzTask.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.Data
{
    public static class StaticResources
    {
        public static Dictionary<TaskStatus, string> TaskStatusUIElement
            = new Dictionary<TaskStatus, string>()
            {
                { TaskStatus.Assiged, "<i class=\"fa fa-anchor\"></i> Assiged" },
                { TaskStatus.Closed, "<i class=\"fa fa-check-circle text-success\"></i> Closed" },
                { TaskStatus.Failed, "<i class=\"fa fa-close text-red\"></i> Failed" },
                { TaskStatus.Feedback, "<i class=\"fa fa-coffee text-fuchsia\"></i> Wait for feedback" },
                { TaskStatus.Open, "<i class=\"fa fa-circle-o text-yellow\"></i> Open" },
                { TaskStatus.Resovled, "<i class=\"fa fa-thumbs-o-up\"></i> Resovled" },
                { TaskStatus.Working, "<i class=\"fa fa-plane text-yellow\"></i> Working" }
            };
    }
}
