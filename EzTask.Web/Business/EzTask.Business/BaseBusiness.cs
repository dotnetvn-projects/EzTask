using EzTask.DataAccess;
using EzTask.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EzTask.Business
{
    public class BaseBusiness<T>
    {
        protected IUnitOfWork<T> UnitOfWork
        {
            get
            {
                return BusinessInitializer.ServiceProvider.GetService<IUnitOfWork<T>>();
            }
        }

        public BaseBusiness()
        {
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
