using EzTask.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Modules.Task.ViewModels
{
    public class TaskItemViewModel: BaseModel
    {
        public int TaskId { get; set; }

        public string TaskCode { get; set; }

        [Required(ErrorMessage = "The Task Title field is required."), StringLength(maximumLength: 255, MinimumLength = 5,
            ErrorMessage = "The Task Title must be a string have length between 5 and 50 characters")]
        public string TaskTitle { get; set; }

        public string TaskDetail { get; set; }

        public int PhaseId { get; set; }

        public short Status { get; set; }

        public int Assignee { get; set; }
        public int AccountId { get; set; }

        public int ProjectId { get; set; }

        public short Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PercentCompleted { get; set; }

        public List<SelectListItem> PhaseList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> AssigneeList { get; set; }
        public List<SelectListItem> PriorityList { get; set; }
    }
}
