﻿using EzTask.Framework.ModelValidatorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Authentication.ViewModels
{
    public class RegisterViewModel: AuthViewModel
    {
        [RequiredField(errorLanguageKey: "PasswordTempValidate", languagePageSetting: "AuthenticationPage"),
         StringLengthField(minimumLength: 6, maximumLength: 50, errorLanguageKey : "PasswordTempValidate", languagePageSetting : "AuthenticationPage")]
        public string PasswordTemp { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public bool IsAcceptPolicy { get; set; }

        public RegisterViewModel()
        {
            IsAcceptPolicy = true;
        }
    }
}
