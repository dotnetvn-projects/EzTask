using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService.Data.Email
{
    public class MailServerSettings
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string AppEmail { get; set; }
        public string AppEmailPass { get; set; }
    }
}
