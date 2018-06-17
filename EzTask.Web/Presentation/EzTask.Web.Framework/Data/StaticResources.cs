using EzTask.Framework.Common;
using EzTask.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.Data
{
    public class StaticResources
    {
        private Dictionary<TaskStatus, string> _taskStatusUIElement;

        public StaticResources ()
        {
            Init();
        }

        private void Init()
        {
            _taskStatusUIElement = new Dictionary<TaskStatus, string>()
            {
                { TaskStatus.Assiged, "<i class=\"fa fa-anchor\"></i> <span class=\"space-left\">Assiged</span>" },
                { TaskStatus.Closed, "<i class=\"fa fa-check-circle text-success\"></i><span class=\"space-left\">Closed</span>" },
                { TaskStatus.Failed, "<i class=\"fa fa-close text-red\"></i> Failed" },
                { TaskStatus.Feedback, "<i class=\"fa fa-coffee text-fuchsia\"></i><span class=\"space-left\">Wait for feedback</span>" },
                { TaskStatus.Open, "<i class=\"fa fa-circle-o text-yellow\"></i><span class=\"space-left\">Open</span>" },
                { TaskStatus.Resovled, "<i class=\"fa fa-thumbs-o-up\"></i><span class=\"space-left\">Resovled</span>" },
                { TaskStatus.Working, "<i class=\"fa fa-plane text-yellow\"></i><span class=\"space-left\">Working</span>" }
            };
        }            

        public string GetTaskStatusUIElement(TaskStatus taskStatus)
        {
            return _taskStatusUIElement[taskStatus];
        }
    }
}
