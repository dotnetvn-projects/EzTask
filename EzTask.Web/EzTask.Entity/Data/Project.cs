using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Project:BaseEntity<Project>
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int MaximumUser { get; set; }
        public Int16 Status { get; set; }
        public string Comment { get; set; }

        public Account Account { get; set; }
    }
}
