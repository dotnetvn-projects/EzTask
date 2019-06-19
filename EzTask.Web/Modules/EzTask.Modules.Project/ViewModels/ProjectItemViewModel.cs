using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model;
using EzTask.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.ViewModels
{
    public class ProjectItemViewModel: BaseModel
    {
        public int ProjectId { get; set; }

        public string ProjectCode { get; set; }

        [RequiredField(ErrorLanguageKey = "ProjectNameValidate", LanguagePageSetting = "ProjectPage")]
        [StringLengthField(maximumLength: 250, MinimumLength = 6, ErrorLanguageKey = "ProjectNameLengthValidate", LanguagePageSetting = "ProjectPage")]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public ProjectStatus Status { get; set; }

        public string Comment { get; set; }
    }
}
