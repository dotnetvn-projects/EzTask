using EzTask.Model;

namespace EzTask.Modules.Task.ViewModels
{
    public class TaskFormDataModel : BaseModel
    {
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int PhaseId { get; set; }
    }
}
