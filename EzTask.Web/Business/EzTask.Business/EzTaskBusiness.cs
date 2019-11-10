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
        public ToDoListBusiness ToDoList { get; }

        public EzTaskBusiness(AccountBusiness account,
            ProjectBusiness project, SkillBusiness skill,
            PhaseBusiness phase, TaskBusiness task,
            NotificationBusiness notification,
            ToDoListBusiness toDoList)
        {
            Account = account;
            Project = project;
            Skill = skill;
            Phase = phase;
            Task = task;
            Notification = notification;
            ToDoList = toDoList;
        }
    }
}
