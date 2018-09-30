using EzTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Task.ViewModels
{
    public class TaskFormDataModel:BaseModel
    {
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int PhraseId { get; set; }
    }
}
