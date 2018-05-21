using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.DataModels
{
    public interface ITaskItemModel
    {
        int Id { get; set; }
        string TaskCode { get; set; }
        string TaskTitle { get; set; }
        string TaskDetail { get; set; }
        IAccountModel Assignee { get; set; }
        IAccountModel Member { get; set; }
        IProjectModel Project { get; set; }
        IProjectModel Phrase { get; set; }
        TaskPriority Priority { get; set; }
        TaskStatus Status { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
