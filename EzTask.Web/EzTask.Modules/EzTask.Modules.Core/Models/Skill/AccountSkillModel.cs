using EzTask.Modules.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Modules.Core.Models.Skill
{
    public class AccountSkillModel : BaseModel
    {
        public SkillModel Skill { get; set; }
        public AccountModel Account { get; set; }
    }
}
