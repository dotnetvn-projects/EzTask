namespace EzTask.Model
{
    public class AccountSkillModel : BaseModel
    {
        public SkillModel Skill { get; set; }
        public AccountModel Account { get; set; }
    }
}
