using EzTask.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace EzTask.Modules.Task.ViewModels
{
    public class TaskViewModel
    {
        public ProjectModel Project { get; set; }
        public List<SelectListItem> ProjectItems { get; set; }
        public IEnumerable<TaskItemModel> TaskList { get; set; }
        public PhaseModel Phase { get; set; }

        public TaskViewModel()
        {
            ProjectItems = new List<SelectListItem>();
            Project = new ProjectModel();
            Phase = new PhaseModel();
        }
    }
}
