using EzTask.Model.Enum;
using EzTask.Framework.ModelValidatorAttributes;
using System;

namespace EzTask.Model
{
    public class AccountModel: BaseModel 
    { 
        public int AccountId { get; set; }
        
        public string AccountName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public DateTime CreatedDate { get; set; }

        public AccountStatus AccountStatus { get; set; }
    }
}
