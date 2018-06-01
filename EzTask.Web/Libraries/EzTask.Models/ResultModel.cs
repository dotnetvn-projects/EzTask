using EzTask.Models.Enum;

namespace EzTask.Models
{
    public class ResultModel
    {
        public ActionStatus Status { get; set; }//move action status to model
        public object Value { get; set; }

    }
}
