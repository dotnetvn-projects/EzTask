using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.DataModels
{
    public interface ISkillModel :IModel
    {
        int Id { get; set; }
        string Skills { get; set; }
    }
}
