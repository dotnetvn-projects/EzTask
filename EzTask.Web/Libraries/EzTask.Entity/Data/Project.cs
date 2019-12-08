using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzTask.Entity.Data
{
    public class Project : Entity<Project>
    {
        [Column(TypeName = "varchar(10)")]
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int MaximumUser { get; set; }
        public Int16 Status { get; set; }
        public string Comment { get; set; }

        [ForeignKey("Owner")]
        public Account Account { get; set; }
    }
}
