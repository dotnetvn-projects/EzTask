﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Management.Models.Account
{
    public class LoginModel:AccountModel
    {
        public bool RememberMe { get; set; }

        public LoginModel()
        {
            RememberMe = true;
        }
    }
}
