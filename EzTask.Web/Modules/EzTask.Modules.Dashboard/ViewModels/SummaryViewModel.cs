using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Dashboard.ViewModels
{
    public class SummaryViewModel
    {
        public int TotalProject { get; set; }
        public int TotalTask { get; set; }
        public int TotalMember { get; set; }
        public int TotalNotification { get; set; }
    }
}
