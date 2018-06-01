using EzTask.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Models
{
    public class PhraseModel:BaseModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required, StringLength(maximumLength: 250, MinimumLength = 5,
            ErrorMessage = "Phrase Name must be a string has from 5 to 250 characters")]
        public string PhraseName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public PhraseStatus Status { get; set; }

        public PhraseModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
            Status = PhraseStatus.Open;
        }
    }
}
