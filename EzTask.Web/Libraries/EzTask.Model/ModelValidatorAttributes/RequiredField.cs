using EzTask.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EzTask.Model.ModelValidatorAttributes
{
    public class RequiredField: RequiredAttribute
    {
        private ILanguageLocalization _languageLocalization;

        public string ErrorLanguageKey { get; set; }
        public string LanguagePageSetting { get; set; }

        public RequiredField(ILanguageLocalization languageLocalization)
        {
            _languageLocalization = languageLocalization;
        }

        public override string FormatErrorMessage(string name)
        {
            return _languageLocalization.GetLocalization(ErrorLanguageKey, LanguagePageSetting);
        }
    }
}
