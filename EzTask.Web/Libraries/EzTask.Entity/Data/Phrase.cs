using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Phrase :Entity<Phrase>
    {
        public int ProjectId { get; set; }
        public string PhraseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short Status { get; set; }
        public bool IsDefault { get; set; }

        public Project Project { get; set; }
    }
}
