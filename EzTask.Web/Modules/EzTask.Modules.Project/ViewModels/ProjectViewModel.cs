using EzTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectModel Project { get; set; }
        public List<List<TaskItemModel>> TaskList { get; set; }
        public int TotalTask { get; set; }
        public int TotalPhrase { get; set; }
        public int TotalMember { get; set; }

        public int TotalOpenPhrase { get; set; }
        public int TotalClosedPhrase { get; set; }
        public int TotalFailedPhrase { get; set; }

        public int PercentOpenPhrase { get; set; }
        public int PercentClosedPhrase { get; set; }
        public int PercentFailedPhrase { get; set; }

        public List<ProjectMemberModel> Members { get; set; }

        public ProjectViewModel()
        {
            Members = new List<ProjectMemberModel>();
            TaskList = new List<List<TaskItemModel>>();
        }
    }
}
