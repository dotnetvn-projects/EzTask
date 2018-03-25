using EzTask.DataAccess;

namespace EzTask.MainBusiness
{
    public class BaseBusiness 
    {
       protected EzTaskDbContext EzTaskDbContext;
       public BaseBusiness(EzTaskDbContext ezTaskDbContext)
       {
            EzTaskDbContext = ezTaskDbContext;
       }
    }
}
