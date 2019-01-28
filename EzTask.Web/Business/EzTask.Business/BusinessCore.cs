using EzTask.DataAccess;
using EzTask.Interface;
using EzTask.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Business
{
    public class BusinessCore
    {
        protected UnitOfWork UnitOfWork { get; }

        public BusinessCore(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
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
