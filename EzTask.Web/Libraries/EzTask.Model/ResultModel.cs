using EzTask.Model.Enum;

namespace EzTask.Model
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
