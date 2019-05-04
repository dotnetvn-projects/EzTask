using EzTask.Interface;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Framework.ModelValidatorAttributes
{
    public class RequiredField: RequiredAttribute
    {
        private ILanguageLocalization _languageLocalization;

        public string ErrorLanguageKey { get; set; }
        public string LanguagePageSetting { get; set; }

        public RequiredField()
        {
            _languageLocalization = FrameworkCore.ServiceProvider.InvokeComponents<ILanguageLocalization>();
        }

        public override string FormatErrorMessage(string name)
        {
            return _languageLocalization.GetLocalization(ErrorLanguageKey, LanguagePageSetting);
        }
    }
}
