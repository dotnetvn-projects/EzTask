using EzTask.Interface.SharedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface ILogger
    {
        void WriteInfo(ILogEntity logData);
        void WriteDebug(ILogEntity logData);
        void WriteError(ILogEntity logData);
    }
}
