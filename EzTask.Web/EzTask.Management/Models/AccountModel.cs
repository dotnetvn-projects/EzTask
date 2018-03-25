using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Management.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }

        [Required, StringLength(maximumLength:50, MinimumLength = 6, 
            ErrorMessage = "Email must be a string between 6 and 50 characters")]
        [EmailAddress]
        [Display(Name ="Email")]
        public string AccountName { get; set; }

        [Required, StringLength(maximumLength: 50, MinimumLength = 6,
            ErrorMessage = "Email must be a string between 6 and 50 characters")]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }

        public bool RememberMe { get; set; }

        public AccountModel()
        {
            RememberMe = true;
        }
    }
}
