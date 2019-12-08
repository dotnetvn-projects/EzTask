using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class TaskItem : Entity<TaskItem>
    {
        [Column(TypeName = "varchar(10)")]
        public string TaskCode { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public int? AssigneeId { get; set; }
        public int MemberId { get; set; }
        public int ProjectId { get; set; }
        public int PhaseId { get; set; }
        public Int16 Priority { get; set; }
        public Int16 Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDate { get; set; }
        public int PercentCompleted { get; set; }
        public int EstimateTime { get; set; }
        public int SpentTime { get; set; }

        public Account Assignee { get; set; }

        public Account Member { get; set; }

        public Project Project { get; set; }

        public Phase Phase { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public TaskItem()
        {
        }
    }
}
