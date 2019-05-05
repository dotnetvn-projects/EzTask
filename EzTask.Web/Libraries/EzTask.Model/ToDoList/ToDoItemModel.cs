using EzTask.Model.Enum;
using System;

namespace EzTask.Model
{
    public class ToDoItemModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ToDoItemPriority Priority { get; set; }
        public ToDoItemStatus Status { get; set; }
        public DateTime CompleteOn { get; set; }
        public AccountModel Account { get; set; }

        public bool IsWarning
        {
            get
            {
                if (CompleteOn.Date < DateTime.Now.Date)
                    return true;
                return false;
            }
        }

        public int TimeLeft { get; set; }

        public ToDoItemModel()
        {
            Priority = ToDoItemPriority.Medium;
            Status = ToDoItemStatus.Waiting;
        }
    }
}
