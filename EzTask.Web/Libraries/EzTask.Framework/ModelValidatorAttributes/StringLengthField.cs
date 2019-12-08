using EzTask.Interface;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Framework.ModelValidatorAttributes
{
    public class StringLengthField : ValidationAttribute, IClientModelValidator
    {
        private ILanguageLocalization _languageLocalization;

        private string _errorLanguageKey { get; set; }
        private string _languagePageSetting { get; set; }

        private int _minimumLength;
        private int _maximumLength;

        public StringLengthField(int minimumLength, int maximumLength,
            string errorLanguageKey, string languagePageSetting)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
            _errorLanguageKey = errorLanguageKey;
            _languagePageSetting = languagePageSetting;
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

            if (isValid && (valueAsString.Length < _minimumLength || valueAsString.Length > _maximumLength))
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
            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-stringlengthfield", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));

            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-stringlengthfield-minLength", _minimumLength.ToString());
            ValidatorUtils.MergeAttribute(context.Attributes, "data-val-stringlengthfield-maxLength", _maximumLength.ToString());
        }
    }
}
