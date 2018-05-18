using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.Models
{
    public interface IAccountModel : IModel
    {
        int AccountId { get; set; }     
        string AccountName { get; set; }
        string Password { get; set; }
        string FullName { get; set; }
        string DisplayName { get; set; }
    }
}
