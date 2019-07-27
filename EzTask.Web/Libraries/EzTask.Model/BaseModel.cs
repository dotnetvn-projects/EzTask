using EzTask.Model.Enum;

namespace EzTask.Model
{
    public class BaseModel
    {
        public bool HasError { get; set; }
        public bool IsSuccess { get; set; }
        public ActionType ActionType { get; set; }
    }
}
