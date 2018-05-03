using EzTask.DataAccess;
using System;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class BaseBusiness
    {
        protected EzTaskDbContext DbContext;
        public BaseBusiness(EzTaskDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected virtual string CreateCode(string prefix, int id)
        {
            if (id < 100 && id > 9)
            {
                return prefix +"0" + id;
            }
            else if (id < 10)
            {
                return prefix+ "00" + id;
            }

            return prefix + id;
        }
    }
}
