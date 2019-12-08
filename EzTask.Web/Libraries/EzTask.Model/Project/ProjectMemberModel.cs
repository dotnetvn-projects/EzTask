using System;

namespace EzTask.Model
{
    public class ProjectMemberModel : BaseModel
    {
        public int ProjectId { get; set; }
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsPending { get; set; }
        public string ActiveCode { get; set; }

        public int TotalTask { get; set; }
    }
}
