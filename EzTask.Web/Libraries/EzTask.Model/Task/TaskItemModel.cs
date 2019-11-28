using EzTask.Model.Enum;
using System;

namespace EzTask.Model
{
    public class TaskItemModel : BaseModel
    {
        public int TaskId { get; set; }
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int PercentCompleted { get; set; }
        public int EstimateTime { get; set; }
        public int SpentTime { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AccountModel Assignee { get; set; }
        public AccountModel Member { get; set; }
        public ProjectModel Project { get; set; }
        public PhaseModel Phase { get; set; }

        public bool HasAttachment { get; set; }

        public TaskItemModel()
        {
            Assignee = new AccountModel();
            Member = new AccountModel();
            Project = new ProjectModel();
            Phase = new PhaseModel();
        }
    }
}
