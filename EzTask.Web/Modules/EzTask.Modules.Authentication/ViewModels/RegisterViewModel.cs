using EzTask.Framework.ModelValidatorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class RegisterViewModel: AuthViewModel
    {
        [RequiredField, StringLengthField(maximumLength: 50, MinimumLength = 6,
            ErrorLanguageKey = "PasswordTempValidate", LanguagePageSetting = "AuthenticationPage")]
        public string PasswordTemp { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public bool IsAcceptPolicy { get; set; }

        public RegisterViewModel()
        {
            IsAcceptPolicy = true;
        }
    }
}
