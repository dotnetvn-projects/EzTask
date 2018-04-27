using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Entity.Data
{
    public class AccountSkill: BaseEntity<AccountSkill>
    {
        public int AccountId { get; set; }
        public int SkillId { get; set; }

        public Account Account { get; set; }
        public Skill Skill { get; set; }
    }
}
