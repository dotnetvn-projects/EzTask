using EzTask.Model;
using EzTask.Model.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EzTask.Model
{
    public class ProjectModel :BaseModel
    {
        public int ProjectId { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public AccountModel Owner { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int MaximumUser { get; set; }

        public ProjectStatus Status { get; set; } 

        public string Comment { get; set; }     

        #region UI Model
        public string Color { get; set; }

        public string BoxType { get; set; }
      
        #endregion
    }
}
