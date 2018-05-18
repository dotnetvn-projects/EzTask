﻿using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.Models
{
    public interface IModel
    {
        bool HasError { get; set; }
        ActionType ActionType { get; set; }
    }
}
