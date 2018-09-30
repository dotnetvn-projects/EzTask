using EzTask.Models;
using EzTask.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EzTask.Models
{
    public class ProjectModel :BaseModel
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Description")]
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
