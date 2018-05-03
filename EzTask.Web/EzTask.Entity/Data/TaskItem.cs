using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class TaskItem : Entity<TaskItem>
    {
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public int? AssigneeId { get; set; }
        public int MemberId { get; set; }
        public int ProjectId { get; set; }
        public Int16 Priority { get; set; }
        public Int16 Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("AssigneeId")]
        public Account Assignee { get; set; }

        [ForeignKey("MemberId")]
        public Account Member { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public TaskItem ()
        {
            Assignee = new Account();
        }
    }
}
