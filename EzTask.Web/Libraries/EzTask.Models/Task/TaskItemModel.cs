using EzTask.Models.Enum;

namespace EzTask.Models
{
    public class TaskItemModel : BaseModel
    {
        public int TaskId { get; set; }
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }    
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public AccountModel Assignee { get; set; }
        public AccountModel Member { get; set; }
        public ProjectModel Project { get; set; }
        public PhraseModel Phrase { get; set; }
    }
}
