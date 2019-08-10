using EzTask.Interface.SharedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface ILogger
    {
        ILogEntity LogEntity { get; set; }
        void WriteInfo();
        void WriteDebug();
        void WriteError();
    }
}
