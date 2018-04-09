using EzTask.Entity.Framework;
using EzTask.Management.Models.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EzTask.Management.Models.Project
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

        public ProjectModel()
        {
            Owner = new AccountModel();
        }
    }
}
