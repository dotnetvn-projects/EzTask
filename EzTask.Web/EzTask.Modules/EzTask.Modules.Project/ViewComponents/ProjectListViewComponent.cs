using EzTask.Modules.Core.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Modules.Project.ViewComponents
{
    public class ProjectListViewComponent : EzTaskViewComponent
    {
        public ProjectListViewComponent(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }


    }
}
