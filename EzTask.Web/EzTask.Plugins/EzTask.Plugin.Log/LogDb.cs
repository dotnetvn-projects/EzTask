﻿using EzTask.Entity.Framework;
using EzTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Plugin.Log
{
    internal class LogDb : ILogger
    {
        public void Write(LogEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
