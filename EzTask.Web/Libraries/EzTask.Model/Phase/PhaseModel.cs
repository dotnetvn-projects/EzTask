using EzTask.Model.Enum;
using System;

namespace EzTask.Model
{
    public class PhaseModel : BaseModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string PhaseName { get; set; }
        public string PhaseGoal { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public PhaseStatus Status { get; set; }

        public bool IsDefault { get; set; }

        public int TotalTask { get; set; }

        public PhaseModel()
        {
            Status = PhaseStatus.Open;
            PhaseName = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
        }
    }
}
