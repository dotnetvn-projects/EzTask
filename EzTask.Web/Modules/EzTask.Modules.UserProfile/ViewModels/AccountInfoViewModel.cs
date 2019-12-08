using EzTask.Framework.ModelValidatorAttributes;

namespace EzTask.Modules.UserProfile.ViewModels
{
    public class AccountInfoViewModel
    {
        public int AccountInfoId { get; set; }

        public int AccountId { get; set; }

        public string JobTitle { get; set; }

        public string Education { get; set; }

        [RequiredField(errorLanguageKey: "FullNameValidate", languagePageSetting: "UserProfilePage")]
        public string FullName { get; set; }

        [RequiredField(errorLanguageKey: "DisplayNameValidate", languagePageSetting: "UserProfilePage")]
        public string DisplayName { get; set; }

        [EmailField(errorLanguageKey: "EmailValidate", languagePageSetting: "UserProfilePage")]
        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PhoneNumber { get; set; }

        public string BirthDay { get; set; }

        public string Comment { get; set; }

        public string Introduce { get; set; }

        public string Skills { get; set; }

        public bool IsPublished { get; set; }

        public AccountInfoViewModel()
        {
            this.IsPublished = true;
        }

    }
}
