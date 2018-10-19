using System;

namespace EzTask.Entity.Data
{
    public class ProjectMember : Entity<ProjectMember>
    {
        public int MemberId { get; set; }
        public int ProjectId { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsPending { get; set; }

        public Account Member { get; set; }
        public Project Project { get; set; }
    }
}
