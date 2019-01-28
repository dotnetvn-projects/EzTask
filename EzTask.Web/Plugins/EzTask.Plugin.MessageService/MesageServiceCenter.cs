using EzTask.Interface;
using EzTask.Plugin.MessageService.Data;
using EzTask.Plugin.MessageService.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Plugin.MessageService
{
    public class MesageServiceCenter
    {
        private IList<IMessageService<Message>> _services;
        public MesageServiceCenter()
        {
            ServiceFactory();
        }

        private void ServiceFactory()
        {
            _services = new List<IMessageService<Message>>
            {
                new EmailMessageService()
            };
        }

        public void Push(Message message)
        {
            var type = message.GetType();
            if (type == typeof(PushMessage))
            {
                var service = _services.First(c => c.GetType() == typeof(EmailMessageService));
                service.Delivery(message);
            }
        }
    }
}
