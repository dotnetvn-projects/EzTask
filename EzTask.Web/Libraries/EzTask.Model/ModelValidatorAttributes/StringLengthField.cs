using EzTask.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Model.ModelValidatorAttributes
{
    public class StringLengthField: StringLengthAttribute
    {
        private ILanguageLocalization _languageLocalization;

        public string ErrorLanguageKey { get; set; }
        public string LanguagePageSetting { get; set; }

        public StringLengthField(int maximumLength, ILanguageLocalization languageLocalization) : base(maximumLength)
        {
            _languageLocalization = languageLocalization;
        }

        public override string FormatErrorMessage(string name)
        {
            return _languageLocalization.GetLocalization(ErrorLanguageKey, LanguagePageSetting);
        }
    }
}
