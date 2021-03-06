﻿using EzTask.Model.Enum;
using System;

namespace EzTask.Model
{
    public class NotificationModel : BaseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public NotifyContext Context { get; set; }
        public string RefData { get; set; }
        public bool HasViewed { get; set; }

        public AccountModel Account { get; set; }
    }
}
