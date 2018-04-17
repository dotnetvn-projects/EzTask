using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzTask.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Delete(int id);
        Task<T> GetById(int id);
        Task<T> Get(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where);
    }
}
