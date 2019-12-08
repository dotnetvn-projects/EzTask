using EzTask.Framework.ModelValidatorAttributes;
using EzTask.Model;
using EzTask.Model.Enum;

namespace EzTask.Modules.Project.ViewModels
{
    public class ProjectItemViewModel : BaseModel
    {
        public int ProjectId { get; set; }

        public string ProjectCode { get; set; }

        [RequiredField(errorLanguageKey: "ProjectNameValidate", languagePageSetting: "ProjectPage")]
        [StringLengthField(minimumLength: 6, maximumLength: 250, errorLanguageKey: "ProjectNameLengthValidate", languagePageSetting: "ProjectPage")]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public ProjectStatus Status { get; set; }

        public string Comment { get; set; }
    }
}
