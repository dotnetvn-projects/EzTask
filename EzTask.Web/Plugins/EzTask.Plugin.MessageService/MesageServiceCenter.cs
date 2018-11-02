using EzTask.Interfaces;
using EzTask.Plugin.MessageService.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.MessageService
{
    public class MesageServiceCenter
    {       
        private IMessageService<PushMessage> _pushService;

        public MesageServiceCenter()
        {
            _pushService = new PushNotifyMessageService();
        }

        public void Delivery(Message message)
        {
            var type = message.GetType();
            if (type == typeof(PushMessage))
            {
                _pushService.Enqueue(message as PushMessage);
            }
        }

        public void Start()
        {
            _pushService.Start();
        }

        public void Stop()
        {
            _pushService.Stop();
        }
    }
}
