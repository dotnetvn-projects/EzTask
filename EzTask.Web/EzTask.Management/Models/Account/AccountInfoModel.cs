using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Management.Models.Account
{
    public class AccountInfoModel:AccountModel
    {
        public int AccountInfoId { get; set; }
        public byte[] DisplayImage { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Comment { get; set; }
        public string Introduce { get; set; }
        public byte[] Document { get; set; }
    }
}
