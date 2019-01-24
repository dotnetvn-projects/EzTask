using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Phase : Entity<Phase>
    {
        public int ProjectId { get; set; }
        public string PhaseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short Status { get; set; }
        public bool IsDefault { get; set; }

        public Project Project { get; set; }
    }
}
