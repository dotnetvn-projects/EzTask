﻿using System;

namespace EzTask.Model
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

        public DateTime? BirthDay { get; set; }

        public string Comment { get; set; }
        public string Introduce { get; set; }
        public byte[] Document { get; set; }
        public string LangDisplay { get; set; }
        public string Skills { get; set; }
        public bool IsPublished { get; set; }

        public AccountInfoModel()
        {
            IsPublished = true;
        }
    }
}
