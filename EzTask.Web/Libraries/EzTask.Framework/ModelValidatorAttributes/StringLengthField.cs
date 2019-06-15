using EzTask.Interface;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Framework.ModelValidatorAttributes
{
    public class StringLengthField : StringLengthAttribute
    {
        private ILanguageLocalization _languageLocalization;

        public string ErrorLanguageKey { get; set; }
        public string LanguagePageSetting { get; set; }

        public StringLengthField(int maximumLength) : base(maximumLength)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _languageLocalization = (ILanguageLocalization)validationContext
                          .GetService(typeof(ILanguageLocalization));
            string valueAsString = value as string;

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                isValid = false;
            }

            if (isValid && (valueAsString.Length < MinimumLength || valueAsString.Length > MaximumLength))
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                   FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return _languageLocalization.GetLocalization(ErrorLanguageKey, LanguagePageSetting);
        }
    }
}
