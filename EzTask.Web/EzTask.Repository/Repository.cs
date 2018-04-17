using EzTask.DataAccess;
using EzTask.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Repository
{
    public abstract class Repository<T> : BaseResopitory<T>, IRepository<T> where T : class
    {
        protected Repository(EzTaskDbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// For adding entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<T> Add(T entity)
        {
            try
            {
                Dbset.Add(entity);
                DataContext.Entry(entity).State = EntityState.Added;
                int iresult = await DataContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For updating entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<T> Update(T entity)
        {
            try
            {
                Dbset.Add(entity);
                DataContext.Entry(entity).State = EntityState.Modified;
                int iresult = await DataContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// for deleting entity with class 
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<bool> Delete(T entity)
        {
            try
            {
                Dbset.Remove(entity);
                int iresult = await DataContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// for deleting entity with primary key 
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                var entity = GetById(id);
                Dbset.Remove(entity);
                int iresult = await DataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Fetches values as per the int64 id value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetById(int id)
        {
            return await Dbset.FindAsync(id);
        }

        /// <summary>
        /// fetches all the records 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await Dbset.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Fetches records as per the predicate condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where)
        {
            return await Dbset.Where(where).ToListAsync();
        }

        /// <summary>
        /// fetches single records as per the predicate condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await Dbset.Where(where).FirstOrDefaultAsync();
        }    
    }
      
}
