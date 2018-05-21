using EzTask.Entity.Framework;
using EzTask.Interfaces.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Core.Models.Account
{
    public class AccountModel: BaseModel, IAccountModel
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
    }
}
