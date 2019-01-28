using EzTask.Model;
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
        public int TotalPhase { get; set; }
        public int TotalMember { get; set; }

        public int TotalOpenPhase { get; set; }
        public int TotalClosedPhase { get; set; }
        public int TotalFailedPhase { get; set; }

        public int PercentOpenPhase { get; set; }
        public int PercentClosedPhase { get; set; }
        public int PercentFailedPhase { get; set; }

        public List<ProjectMemberModel> Members { get; set; }

        public ProjectViewModel()
        {
            Members = new List<ProjectMemberModel>();
            TaskList = new List<List<TaskItemModel>>();
        }
    }
}
