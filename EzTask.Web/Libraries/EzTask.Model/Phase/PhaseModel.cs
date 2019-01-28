using EzTask.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Model
{
    public class PhaseModel:BaseModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required, StringLength(maximumLength: 250, MinimumLength = 5,
            ErrorMessage = "Phase Name must be a string has from 5 to 250 characters")]
        public string PhaseName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]
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
