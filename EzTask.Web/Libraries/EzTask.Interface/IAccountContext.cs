using EzTask.Interface.SharedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interface
{
    public interface IAccountContext
    {
        void Set(IAccountInfo accountInfo);

        bool IsLogined { get;}

        /// <summary>
        /// Account Id
        /// </summary>
        int AccountId { get;}

        /// <summary>
        /// DisplayName
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// AccountName
        /// </summary>
        string AccountName { get; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// JobTitle
        /// </summary>
        string JobTitle { get; }

    }
}
