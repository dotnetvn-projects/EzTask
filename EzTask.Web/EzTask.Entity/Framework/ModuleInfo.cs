using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzTask.Entity.Framework
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
        public string Path { get; set; }
    }
}
