﻿using EzTask.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Model
{
    public class AccountSkillModel : BaseModel
    {
        public SkillModel Skill { get; set; }
        public AccountModel Account { get; set; }
    }
}