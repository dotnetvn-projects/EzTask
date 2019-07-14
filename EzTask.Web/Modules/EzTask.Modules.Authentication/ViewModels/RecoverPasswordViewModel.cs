using EzTask.Framework.ModelValidatorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [RequiredField(errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
           StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
           EmailField(errorLanguageKey: "EmailValidate", languagePageSetting: "AuthenticationPage")]
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
