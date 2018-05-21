namespace EzTask.Models
{
    public class TaskItemModel : BaseModel
    {
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }        
    }
}
