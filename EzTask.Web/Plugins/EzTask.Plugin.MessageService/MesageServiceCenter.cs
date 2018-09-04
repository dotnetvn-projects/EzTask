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
        private IMessageService<TaskHistoryMessage> _taskHistoryService;
        private IMessageService<PushMessage> _pushService;

        public MesageServiceCenter()
        {
            _taskHistoryService = new TaskHistoryMessageService();
            _pushService = new PushNotifyMessageService();
        }

        public void Delivery(Message message)
        {
            var type = message.GetType();
            if(type == typeof(TaskHistoryMessage))
            {
                _taskHistoryService.Enqueue(message as TaskHistoryMessage);
            }
            else if(type == typeof(PushMessage))
            {
                _pushService.Enqueue(message as PushMessage);
            }
        }

        public void Start()
        {
            _taskHistoryService.Start();
            _pushService.Start();
        }

        public void Stop()
        {
            _taskHistoryService.Stop();
            _pushService.Stop();
        }
    }
}
