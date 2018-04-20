using EzTask.Business.BusinessAreas;
using System.Diagnostics;

namespace EzTask.Business.BusinessAreas
{
    public class EzTaskBusiness 
    {
        public AccountBusiness Account { get; }
        public ProjectBusiness Project { get; }

        public EzTaskBusiness(AccountBusiness accountBusiness,
            ProjectBusiness projectBusiness)
        {
            Account = accountBusiness;
            Project = projectBusiness;
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
