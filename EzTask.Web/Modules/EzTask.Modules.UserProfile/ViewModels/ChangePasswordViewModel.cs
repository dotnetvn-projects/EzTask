using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model;

namespace EzTask.Modules.UserProfile.ViewModels
{
    public class ChangePasswordViewModel: BaseModel
    {
        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string CurrentPassword { get; set; }

        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string NewPassword { get; set; }

        [RequiredField(errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey: "PasswordValidate", languagePageSetting: "AuthenticationPage")]
        public string ConfirmNewPassword { get; set; }

        public ChangePasswordViewModel()
        {
            HasError = false;
        }
    }
}
