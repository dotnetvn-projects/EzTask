using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Interfaces.Repository
{
    public interface IQueryRepository<T>
    {
        IEnumerable<T> Query(string queryString);
    }
}
