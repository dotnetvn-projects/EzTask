using EzTask.Interfaces.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.Models.Skill
{
    public class SkillModel:BaseModel, ISkillModel
    {
        public int Id { get; set; }
        public string Skills { get; set; }
    }
}
