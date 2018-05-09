using EzTask.Business;
using System.Diagnostics;

namespace EzTask.Business
{
    public class EzTaskBusiness 
    {
        public AccountBusiness Account { get; }
        public ProjectBusiness Project { get; }
        public SkillBusiness Skill { get;  }
        public PhraseBusiness Phrase { get; }

        public EzTaskBusiness(AccountBusiness account,
            ProjectBusiness project, SkillBusiness skill,
            PhraseBusiness phrase)
        {
            Account = account;
            Project = project;
            Skill = skill;
            Phrase = phrase;
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
