using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Models
{
    public class ResultModel
    {
        public ActionStatus Status { get; set; }
        public object Value { get; set; }

    }
}
