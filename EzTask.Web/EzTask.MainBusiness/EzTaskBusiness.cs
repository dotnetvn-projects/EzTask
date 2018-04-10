using EzTask.DataAccess;
using EzTask.Interfaces;
using System.Diagnostics;

namespace EzTask.MainBusiness
{
    public class EzTaskBusiness : IEzTaskBusiness
    {
        public AccountBusiness Account { get; }
        public ProjectBusiness Project { get; }

        public EzTaskBusiness(EzTaskDbContext ezTaskDbContext)
        {
            Account = new AccountBusiness(ezTaskDbContext);
            Project = new ProjectBusiness(ezTaskDbContext);
        }

        /// <summary>
        /// Track message
        /// </summary>
        public void TrackMe()
        {
            Debug.Print("I am EzTask and I am good to work now :)");
        }
    }
}
