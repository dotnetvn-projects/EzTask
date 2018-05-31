using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces
{
    public interface ILogger
    {
        void Write(LogEntity entity);
    }
}
