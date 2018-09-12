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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public PhraseStatus Status { get; set; }

        public string StartDateDisplay { get; set; }
        public string EndDateDisplay { get; set; }

        public PhraseModel()
        {
            Reset();
            Status = PhraseStatus.Open;
            
        }

        public void Reset()
        {
            PhraseName = string.Empty;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
            StartDateDisplay = StartDate.Value.ToString("dd/MM/yyyy");
            EndDateDisplay = EndDate.Value.ToString("dd/MM/yyyy");
        }
    }
}
