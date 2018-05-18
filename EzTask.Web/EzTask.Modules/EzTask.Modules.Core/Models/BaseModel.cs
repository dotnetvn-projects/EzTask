using EzTask.Entity.Framework;
using EzTask.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Modules.Core.Models
{
    public class BaseModel : IModel
    {
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
