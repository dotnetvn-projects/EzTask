using EzTask.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Models
{
    public class AccountSkillModel : BaseModel
    {
        public SkillModel Skill { get; set; }
        public AccountModel Account { get; set; }
    }
}
