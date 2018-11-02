using EzTask.Framework.Data;
using EzTask.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService.Data
{
    public class TaskNotifyMessage : Message
    {
        public int TaskId { get; set; }
        public CurrentAccount CurrentAccount { get; set; }
        public NotifyContext NotifyContext { get; set; }
    }
}
