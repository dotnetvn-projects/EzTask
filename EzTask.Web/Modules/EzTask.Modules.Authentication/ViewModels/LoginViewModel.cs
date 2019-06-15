using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class LoginViewModel: AuthViewModel
    {
        public bool RememberMe { get; set; }

        public string RedirectUrl { get; set; }

        public LoginViewModel()
        {
            RememberMe = true;
        }
    }
}
