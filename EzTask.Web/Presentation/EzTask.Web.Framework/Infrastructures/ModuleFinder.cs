using EzTask.Web.Framework.Data;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace EzTask.Web.Framework.Infrastructures
{
    public class ModuleFinder
    {
        public IHostingEnvironment _environment;
        public ModuleFinder(IHostingEnvironment env)
        {
            _environment = env;
        }

        internal List<ModuleInfo> Find()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>();
            var moduleRootFolder = new DirectoryInfo(Path.Combine(_environment.ContentRootPath, "Modules"));
            var moduleFolders = moduleRootFolder.GetDirectories();
            foreach (var moduleFolder in moduleFolders)
            {
                var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                if (!binFolder.Exists)
                {
                    continue;
                }

                foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException ex)
                    {
                        if (ex.Message == "Assembly with same name is already loaded")
                        {
                            // Get loaded assembly
                            assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (assembly.FullName.Contains(moduleFolder.Name))
                    {
                        modules.Add(new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName });
                    }
                }
            }
            return modules;
        }
    }
}
