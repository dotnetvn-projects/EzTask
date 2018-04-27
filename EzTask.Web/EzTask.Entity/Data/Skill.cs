using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class Skill : BaseEntity<Skill>
    {
        public string SkillName { get; set; }
    }
}
