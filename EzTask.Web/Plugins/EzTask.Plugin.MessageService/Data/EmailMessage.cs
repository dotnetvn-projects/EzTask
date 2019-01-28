using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService.Data
{
    public class EmailMessage: Message
    {
        public string Host { get; set; }
        public string Sender { get; set; }
        public string SenderPassword { get;set }
        public string To { get; set; }
    }
}
