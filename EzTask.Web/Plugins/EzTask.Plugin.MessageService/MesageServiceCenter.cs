﻿using EzTask.Interface;
using EzTask.Interface.SharedData;
using EzTask.Plugin.MessageService.Data.Email;
using EzTask.Plugin.MessageService.Service;
using System.Collections.Generic;
using System.Linq;

namespace EzTask.Plugin.MessageService
{
    internal class MesageServiceCenter : IMessageCenter
    {
        private IList<IMessageService<IMessage>> _services;
        public MesageServiceCenter()
        {
            ServiceFactory();
        }

        private void ServiceFactory()
        {
            _services = new List<IMessageService<IMessage>>
            {
                new EmailMessageService()
            };
        }

        public void Push(IMessage message)
        {
            var type = message.GetType();

            if (type == typeof(EmailMessage))
            {
                var service = _services.First(c => c.GetType() == typeof(EmailMessageService));
                service.Delivery(message);
            }
        }
    }
}
