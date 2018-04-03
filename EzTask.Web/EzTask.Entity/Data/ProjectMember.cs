using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class ProjectMember:BaseEntity<ProjectMember>
    {
        public int MemberId { get; set; }
        public int ProjectId { get; set; }
        public DateTime AddDate { get; set; }

        public Account Member { get; set; }
        public Project Project { get; set; }
    }
}
