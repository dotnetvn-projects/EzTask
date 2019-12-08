using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class TaskHistory : Entity<TaskHistory>
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public int UpdatedUser { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedDate { get; set; }

        public TaskItem Task { get; set; }

        [ForeignKey("UpdatedUser")]
        public Account User { get; set; }
    }
}
