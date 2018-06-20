using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.Models
{
    public class TaskItemViewModel
    {
        public int TaskId { get; set; }
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }

        public List<SelectListItem> PhraseList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> AssigneeList { get; set; }
    }
}
