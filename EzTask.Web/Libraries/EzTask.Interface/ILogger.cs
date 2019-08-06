using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface ILogger<T>
    {
        void WriteInfo(T logData);
        void WriteDebug(T logData);
        void WriteError(T logData);
    }
}
