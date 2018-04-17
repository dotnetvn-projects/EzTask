using EzTask.DataAccess;

namespace EzTask.Business
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
