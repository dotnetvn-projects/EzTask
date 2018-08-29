using System;
using System.Collections.Generic;
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
        public int PhraseId { get; set; }
        public Int16 Priority { get; set; }
        public Int16 Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Account Assignee { get; set; }

        public Account Member { get; set; }

        public Project Project { get; set; }

        public Phrase Phrase { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public TaskItem ()
        {
            Assignee = new Account();
        }
    }
}
