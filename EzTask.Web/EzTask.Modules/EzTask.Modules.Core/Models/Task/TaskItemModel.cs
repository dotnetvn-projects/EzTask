using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.Models.Task
{
    public class TaskItemModel : BaseModel
    {
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
    }
}
