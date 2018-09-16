using EzTask.Models.Enum;

namespace EzTask.Models
{
    public class ResultModel<T>
    {
        public ActionStatus Status { get; set; }
        public T Data { get; set; }

        public ResultModel()
        {
            Status = ActionStatus.Failed;
        }
    }
}
