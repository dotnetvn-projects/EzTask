using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzTask.Web.Framework.Infrastructures
{
    public class ModuleViewLocationExpander  :  IViewLocationExpander
    {
        private const string _moduleKey = "module";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey(_moduleKey))
            {
                var module = context.Values[_moduleKey];
                if (!string.IsNullOrWhiteSpace(module))
                {
                    var moduleViewLocations = new string[]
                    {
                       "/Modules/" + module + "/Views/{1}/{0}.cshtml",
                       "/Modules/" + module + "/Views/Shared/{0}.cshtml",
                       "/Modules/" + module + "/Views/Shared/Components/{0}.cshtml" ,
                       "/Modules/EzTask.Modules.Core/Views/Shared/{0}.cshtml",
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controller = context.ActionContext.ActionDescriptor.DisplayName;
            var module = controller.Split('.').Take(3);
            var name = String.Join(".", module);

            if (name.Contains("EzTask.Modules"))
            {
                context.Values[_moduleKey] = name;
            }
        }
    }
}
