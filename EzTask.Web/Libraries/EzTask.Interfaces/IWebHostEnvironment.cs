using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces
{
    public interface IWebHostEnvironment
    {
        /// <summary>
        /// Root path of application
        /// </summary>
        /// <returns></returns>
       string GetRootUrl();

        /// <summary>
        /// www folder of application
        /// </summary>
        /// <returns></returns>
       string GetRootContentUrl();
    }
}
