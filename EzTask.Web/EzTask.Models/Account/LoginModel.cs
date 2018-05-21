using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Models
{
    public class LoginModel:AccountModel
    {
        public bool RememberMe { get; set; }

        public string RedirectUrl { get; set; }

        public LoginModel()
        {
            RememberMe = true;
        }
    }
}
