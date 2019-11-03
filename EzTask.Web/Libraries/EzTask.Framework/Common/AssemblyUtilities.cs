using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace EzTask.Framework.Common
{
    public class AssemblyUtilities
    {
        public static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                try
                {
                    if (module.FileName.ToLower().Contains("eztask"))
                    {
                        var assemblyName = AssemblyLoadContext.GetAssemblyName(module.FileName);
                        var assembly = Assembly.Load(assemblyName);
                        assemblies.Add(assembly);
                    }
                }
                catch (BadImageFormatException)
                {
                    // ignore native modules
                }
            }

            return assemblies.ToArray();
        }
    }
}
