using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Tasks.Models
{
    public class TaskListModel
    {
        public List<SelectListItem> ProjectItems { get; set; }

        public TaskListModel()
        {
            ProjectItems = new List<SelectListItem>();
        }
    }
}
