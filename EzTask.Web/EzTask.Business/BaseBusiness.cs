using EzTask.DataAccess;
using System;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class BaseBusiness 
    {
       protected EzTaskDbContext DbContext;
       public BaseBusiness(EzTaskDbContext ezTaskDbContext)
       {
            DbContext = ezTaskDbContext;
       }
    }
}
