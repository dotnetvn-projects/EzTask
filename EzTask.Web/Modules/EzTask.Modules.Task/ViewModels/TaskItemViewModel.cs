using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EzTask.Modules.Task.ViewModels
{
    public class TaskItemViewModel : BaseModel
    {
        public int TaskId { get; set; }

        public string TaskCode { get; set; }

        [RequiredField(errorLanguageKey: "TaskTitleValidate", languagePageSetting: "TaskPage"),
         StringLengthField(minimumLength: 5, maximumLength: 50, errorLanguageKey: "TaskTitleLengthValidate", languagePageSetting: "TaskPage")]
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

        [RangeField(0, int.MaxValue, errorLanguageKey: "TaskEstimateTimeValidate", languagePageSetting: "TaskPage")]
        public int EstimateTime { get; set; }

        [RangeField(0, int.MaxValue, errorLanguageKey: "TaskSpentTimeValidate", languagePageSetting: "TaskPage")]
        public int SpentTime { get; set; }

        public List<SelectListItem> PhaseList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> AssigneeList { get; set; }
        public List<SelectListItem> PriorityList { get; set; }
    }
}
