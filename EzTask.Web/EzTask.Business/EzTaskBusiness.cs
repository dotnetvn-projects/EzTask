using EzTask.Business;
using System.Diagnostics;

namespace EzTask.Business
{
    public class EzTaskBusiness 
    {
        public AccountBusiness Account { get; }
        public ProjectBusiness Project { get; }
        public SkillBusiness Skill { get; set; }

        public EzTaskBusiness(AccountBusiness account,
            ProjectBusiness project, SkillBusiness skill)
        {
            Account = account;
            Project = project;
            Skill = skill;
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
