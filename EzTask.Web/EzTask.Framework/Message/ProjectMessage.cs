using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Message
{
    public class ProjectMessage
    {
        public const string ErrorCreateProject = "Sorry we cannot create project for you, please try again !";
        public const string CreateProjectSuccess = "Your project has been created !";
        public const string UpdateProjectSuccess = "Your project has been updated !";
        public const string ProjectIsDupplicated = "Project name was register by other user, please choose another name";
        public const string ErrorUpdateProject = "Sorry we cannot update project for you, please try again !";
    }
}
