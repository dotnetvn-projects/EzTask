using EzTask.Interface.SharedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface IMessageCenter
    {
        void Push(IMessage message);
    }
}
