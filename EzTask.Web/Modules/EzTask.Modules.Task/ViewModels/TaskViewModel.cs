using EzTask.Models;
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
        public PhraseModel Phrase { get; set; }

        public string TaskCount
        {
            get
            {
                var count = TaskList.Count();
                if (count > 1)
                    return count + " tasks";
                return count + " task";
            }
        }

        public TaskViewModel()
        {
            ProjectItems = new List<SelectListItem>();
            Project = new ProjectModel();
            Phrase = new PhraseModel();
        }
    }
}
