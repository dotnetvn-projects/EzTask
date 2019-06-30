using EzTask.Framework.ModelValidatorAttributes;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class AuthViewModel
    {
        [RequiredField(errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
         EmailField(errorLanguageKey: "EmailValidate", languagePageSetting: "AuthenticationPage")]
        public string AccountName { get; set; }

        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string Password { get; set; }
    }
}
