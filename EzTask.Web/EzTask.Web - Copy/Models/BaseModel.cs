using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Web.Models
{
    public class BaseModel
    {
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
