using EzTask.Model.Enum;
using System;

namespace EzTask.Model.ToDoList
{
    public class ToDoItemModel: BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ToDoItemStatus Status { get; set; }
        public DateTime CompleteOn { get; set; }
        public AccountModel Account { get; set; }

        public ToDoItemModel()
        {
            Status = ToDoItemStatus.New;
        }
    }
}
