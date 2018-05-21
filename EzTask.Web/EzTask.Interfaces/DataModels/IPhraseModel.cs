using EzTask.Entity.Framework;
using System;

namespace EzTask.Interfaces.DataModels
{
    public interface IPhraseModel : IModel
    {
        int Id { get; set; }

        int ProjectId { get; set; }

        string PhraseName { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }

        PhraseStatus Status { get; set; }
    }
}
