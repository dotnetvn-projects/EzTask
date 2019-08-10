using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface.SharedData
{
    public interface ILogEntity
    {
        string Message { get; }
        Exception Exception { get; }
        string AccountName { get; }
        string Function { get; }
    }
}
