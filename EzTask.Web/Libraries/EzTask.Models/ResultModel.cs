using EzTask.Entity.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Models
{
    public class ResultModel
    {
        public ActionStatus Status { get; set; }//move action status to model
        public object Value { get; set; }

    }
}
