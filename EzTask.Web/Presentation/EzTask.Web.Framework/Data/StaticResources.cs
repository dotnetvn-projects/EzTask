using EzTask.Framework.Common;
using EzTask.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Web.Framework.Data
{
    public class StaticResources
    {
        private Dictionary<TaskItemStatus, string> _taskStatusUIElement;

        public StaticResources ()
        {
            Init();
        }

        private void Init()
        {
            _taskStatusUIElement = new Dictionary<TaskItemStatus, string>()
            {
                { TaskItemStatus.Assiged, "<i class=\"fa fa-anchor\"></i> <span class=\"space-left\">Assiged</span>" },
                { TaskItemStatus.Closed, "<i class=\"fa fa-check-circle text-success\"></i><span class=\"space-left\">Closed</span>" },
                { TaskItemStatus.Failed, "<i class=\"fa fa-close text-red\"></i> Failed" },
                { TaskItemStatus.Feedback, "<i class=\"fa fa-coffee text-fuchsia\"></i><span class=\"space-left\">Wait for feedback</span>" },
                { TaskItemStatus.Open, "<i class=\"fa fa-circle-o text-yellow\"></i><span class=\"space-left\">Open</span>" },
                { TaskItemStatus.Resovled, "<i class=\"fa fa-thumbs-o-up\"></i><span class=\"space-left\">Resovled</span>" },
                { TaskItemStatus.Working, "<i class=\"fa fa-plane text-yellow\"></i><span class=\"space-left\">Working</span>" }
            };
        }            

        public string GetTaskStatusUIElement(TaskItemStatus taskStatus)
        {
            return _taskStatusUIElement[taskStatus];
        }
    }
}
