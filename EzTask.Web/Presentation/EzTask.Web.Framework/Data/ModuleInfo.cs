using System.Reflection;

namespace EzTask.Web.Framework.Data
{
    internal class ModuleInfo
    {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
        public string Path { get; set; }
    }
}