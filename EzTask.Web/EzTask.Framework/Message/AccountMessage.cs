﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Message
{
    public class AccountMessage
    {
        public static readonly string CreateFailed = "Sorry we cannot create account for you, please try again";
        public static readonly string LoginFailed = "Failed to login, wrong email or password";
        public static readonly string AccountBlock = "Failed to login, your account is blocked";
        public static readonly string ExistAccount = "This email already registered, please choose an other email";
        public static readonly string ConfirmPasswordNotMatch = "Value in \"Retype Password\" field doesn't match to your password, please re-type again";
    }
}
