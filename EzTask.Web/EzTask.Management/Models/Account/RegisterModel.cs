using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Management.Models.Account
{
    public class RegisterModel:AccountModel
    {
        [Required, StringLength(maximumLength: 50, MinimumLength = 6,
            ErrorMessage = "Email must be a string between 6 and 50 characters")]
        public string PasswordTemp { get; set; }
        
        public bool IsAcceptPolicy { get; set; }

        public RegisterModel()
        {
            IsAcceptPolicy = true;
        }
    }
}
