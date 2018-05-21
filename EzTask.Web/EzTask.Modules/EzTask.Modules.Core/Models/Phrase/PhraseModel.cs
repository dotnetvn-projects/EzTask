using EzTask.Entity.Framework;
using EzTask.Interfaces.DataModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Modules.Core.Models.Phrase
{
    public class PhraseModel:BaseModel, IPhraseModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required, StringLength(maximumLength: 250, MinimumLength = 5,
            ErrorMessage = "Phrase Name must be a string has from 5 to 250 characters")]
        public string PhraseName { get; set; }

        public DateTime? StartDate { get; set; }

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
