using EzTask.DataAccess;
using EzTask.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Repository
{
    public class QueryRepository<T> : BaseResopitory<T>, IQueryRepository<T> where T : class
    {
        protected QueryRepository(EzTaskDbContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<T> Query(string queryString)
        {
            throw new NotImplementedException();
        }
    }
}
