using System;
using System.ComponentModel.DataAnnotations;

namespace EzTask.Models
{
    public class AccountInfoModel : AccountModel
    {
        public int AccountInfoId { get; set; }
        public string JobTitle { get; set; }
        public string Education { get; set; }
        public byte[] DisplayImage { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }

        public string Comment { get; set; }
        public string Introduce { get; set; }
        public byte[] Document { get; set; }
        public string Skills { get; set; }
        public bool IsPublished { get; set; }

        public AccountInfoModel()
        {
            IsPublished = true;
        }   
    }
}
