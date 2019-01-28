using EzTask.Model.Enum;

namespace EzTask.Model
{
    public class BaseModel
    {
        public string Key { get; set; }
        public bool HasError { get; set; }
        public ActionType ActionType { get; set; }
    }
}
