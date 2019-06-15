using EzTask.Framework.ModelValidatorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class AuthViewModel
    {
        [RequiredField, StringLengthField(maximumLength:20, MinimumLength = 6,
            ErrorLanguageKey = "AccountValidate", LanguagePageSetting = "AuthenticationPage")]
        [EmailField(ErrorLanguageKey = "EmailValidate", LanguagePageSetting = "AuthenticationPage")]
        public string AccountName { get; set; }

        [RequiredField, StringLengthField(maximumLength: 50, MinimumLength = 6,
            ErrorLanguageKey = "PasswordValidate", LanguagePageSetting = "AuthenticationPage")]
        public string Password { get; set; }
    }
}
