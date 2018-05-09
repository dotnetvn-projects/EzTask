using EzTask.Entity.Framework;
using System;

namespace EzTask.Modules.Core.Models.Phrase
{
    public class PhraseModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string PhraseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PhraseStatus Status { get; set; }
    }
}
