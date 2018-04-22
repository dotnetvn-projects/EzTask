using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.Models
{
    public class BaseModel
    {
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
