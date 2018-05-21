using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.DataModels
{
    public interface IProjectModel:IModel
    {
        int ProjectId { get; set; }
        string ProjectCode { get; set; }
        string ProjectName { get; set; }
        string Description { get; set; }
        IAccountModel Owner { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        int MaximumUser { get; set; }
        ProjectStatus Status { get; set; }
        string Comment { get; set; }
    }
}
