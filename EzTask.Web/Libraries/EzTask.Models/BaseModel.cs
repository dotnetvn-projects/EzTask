using EzTask.Models.Enum;

namespace EzTask.Models
{
    public class BaseModel
    {
        public string Key { get; set; }
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
