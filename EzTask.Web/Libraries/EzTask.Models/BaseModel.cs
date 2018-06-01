using EzTask.Models.Enum;

namespace EzTask.Models
{
    public class BaseModel
    {
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
