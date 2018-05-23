using EzTask.Entity.Framework;

namespace EzTask.Models
{
    public class BaseModel
    {
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
