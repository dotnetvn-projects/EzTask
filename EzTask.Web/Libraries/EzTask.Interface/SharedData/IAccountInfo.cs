using System;

namespace EzTask.Interface.SharedData
{
    public interface IAccountInfo
    {
        int AccountId { get; set; }

        string AccountName { get; set; }
        string DisplayName { get; set; }
        string JobTitle { get; set; }
        string Language { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
