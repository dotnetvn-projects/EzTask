using EzTask.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public int PhraseId { get; set; }

        public Int16 Status { get; set; }

        public int Assignee { get; set; }
        public int AccountId { get; set; }

        public int ProjectId { get; set; }

        public Int16 Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PercentCompleted { get; set; }

        public List<SelectListItem> PhraseList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> AssigneeList { get; set; }
        public List<SelectListItem> PriorityList { get; set; }
    }
}
