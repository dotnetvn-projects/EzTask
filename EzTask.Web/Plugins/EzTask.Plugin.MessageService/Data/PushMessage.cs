using EzTask.Interface.SharedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService.Data
{
    public class PushMessage : IMessage
    {
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
