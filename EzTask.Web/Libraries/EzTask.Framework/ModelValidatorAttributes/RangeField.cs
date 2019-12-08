using EzTask.Interface;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Framework.ModelValidatorAttributes
{
    public class RangeField : ValidationAttribute, IClientModelValidator
    {
        private ILanguageLocalization _languageLocalization;

        private string _errorLanguageKey { get; set; }
        private string _languagePageSetting { get; set; }

        private int _minimum;
        private int _maximum;

        public RangeField(int minimun, int maximun,
            string errorLanguageKey, string languagePageSetting)
        {
            _minimum = minimun;
            _maximum = maximun;
            _errorLanguageKey = errorLanguageKey;
            _languagePageSetting = languagePageSetting;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _languageLocalization = (ILanguageLocalization)validationContext
                          .GetService(typeof(ILanguageLocalization));

            int valueAsInt;
            int.TryParse(value as string, out valueAsInt);

            bool isValid = true;

            if (valueAsInt < _minimum || valueAsInt > _maximum)
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
            return _languageLocalization.GetLocalization(_errorLanguageKey, _languagePageSetting);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _languageLocalization = context.ActionContext.
                                HttpContext.RequestServices.InvokeComponents<ILanguageLocalization>();

            ValidatorUtils.MergeAttribute(context.Attributes, "data-val", "true");
            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-rangefield", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));

            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-rangefield-min", _maximum.ToString());
            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-rangefield-max", _maximum.ToString());
        }
    }
}
