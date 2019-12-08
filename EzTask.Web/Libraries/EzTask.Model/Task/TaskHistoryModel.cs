using System;

namespace EzTask.Model
{
    public class TaskHistoryModel : BaseModel
    {
        public int HistoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedDate { get; set; }
        public AccountModel User { get; set; }
        public TaskItemModel Task { get; set; }

        public TaskHistoryModel()
        {
            User = new AccountModel();
            Task = new TaskItemModel();
        }
    }
}
