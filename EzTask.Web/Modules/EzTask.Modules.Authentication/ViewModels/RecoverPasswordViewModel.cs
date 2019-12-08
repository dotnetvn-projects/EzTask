using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class RecoverPasswordViewModel : BaseModel
    {
        [RequiredField(errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
           StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "AccountValidate", languagePageSetting: "AuthenticationPage"),
           EmailField(errorLanguageKey: "EmailValidate", languagePageSetting: "AuthenticationPage")]
        public string Email { get; set; }

        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string Password { get; set; }

        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string ConfirmPassword { get; set; }

        public string AccountName { get; set; }
        public string RecoverCode { get; set; }
        public bool IsExpired { get; set; }
    }
}
