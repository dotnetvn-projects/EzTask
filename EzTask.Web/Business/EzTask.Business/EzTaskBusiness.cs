using System.Diagnostics;

namespace EzTask.Business
{
    public class EzTaskBusiness 
    {
        public AccountBusiness Account { get; }
        public ProjectBusiness Project { get; }
        public SkillBusiness Skill { get;  }
        public PhaseBusiness Phase { get; }
        public TaskBusiness Task { get; }
        public NotificationBusiness Notification { get; }

        public EzTaskBusiness(AccountBusiness account,
            ProjectBusiness project, SkillBusiness skill,
            PhaseBusiness phase, TaskBusiness task,
            NotificationBusiness notification)
        {
            Account = account;
            Project = project;
            Skill = skill;
            Phase = phase;
            Task = task;
            Notification = notification;
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
