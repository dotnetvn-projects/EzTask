using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.ViewModels
{
    public class TaskViewModel
    {
        public List<SelectListItem> ProjectItems { get; set; }

        public TaskViewModel()
        {
            ProjectItems = new List<SelectListItem>();
        }
    }
}
