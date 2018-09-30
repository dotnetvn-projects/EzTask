using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Models
{
    public class ProjectMemberModel: BaseModel
    {
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
        public DateTime AddDate { get; set; }
        public int TotalTask { get; set; }
    }
}
