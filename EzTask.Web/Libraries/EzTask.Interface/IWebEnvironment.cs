﻿namespace EzTask.Interface
{
    public interface IWebEnvironment
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
