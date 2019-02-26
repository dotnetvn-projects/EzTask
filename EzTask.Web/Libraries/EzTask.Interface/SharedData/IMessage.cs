using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface.SharedData
{
    public interface IMessage
    {
        string Title { get; set; }
        string Content { get; set; }
    }
}
