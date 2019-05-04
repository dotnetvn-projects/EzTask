using EzTask.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EzTask.Repository
{
    /// <summary>
    /// Repository Core
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TRepository<T> : IRepository<T> where T : class
    {   
        public TRepository()
        {
        }

        public DbSet<T> Entity
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public DbContext Context { get; set; }

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Entity.Add(entity);
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            var key = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            T existing = Entity.Find(key);
            if (existing != null)
                Entity.Remove(existing);
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="key"></param>
        public void Delete(int key)
        {
            T existing = Entity.Find(key);
            if (existing != null)
                Entity.Remove(existing);
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<T> entities)
        {
            Entity.RemoveRange(entities);
        }

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id, bool allowTracking = true)
        {
            return Entity.FirstOrDefault(c => 
            ((int)c.GetType().GetProperty("Id").GetValue(c) == id));
        }

        /// <summary>
        /// Get entity by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> predicate, bool allowTracking = true)
        {
            return Entity.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get list of entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll(bool allowTracking = true)
        {
            return Entity.AsEnumerable();
        }

        /// <summary>
        /// Get entites by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool allowTracking = true)
        {
            return Entity.Where(predicate).AsEnumerable();
        }

        /// <summary>
        /// Get paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="allowTracking"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPaging(Expression<Func<T, bool>> predicate, int page, int pageSize, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.Where(predicate).Skip(page).Take(pageSize).AsEnumerable();
            }

            return Entity.AsNoTracking().Where(predicate).Skip(page).Take(pageSize).AsEnumerable();
        }

        /// <summary>
        /// Count entities by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate,
            bool allowTracking = true)
        {
            var data = Entity.Count(predicate);
            return data;
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Entity.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;         
        }

        /// <summary>
        /// Get entities from sql string 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<T> FromSqlQuery(string sql, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.FromSql(sql).AsEnumerable();
            }

            return Entity.AsNoTracking().FromSql(sql).AsEnumerable();
        }

        /// <summary>
        /// Get all entities async
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync(bool allowTracking = true)
        {
            var data = await Entity.ToListAsync();
            return data;
        }

        /// <summary>
        /// Get entities by lambda expression async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate,
            bool allowTracking = true)
        {
            var data = await Entity.Where(predicate).ToListAsync();
            return data;
        }

        /// <summary>
        /// Count entities by lambda expression async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate,
            bool allowTracking = true)
        {
            var data = await Entity.CountAsync(predicate);
            return data;
        }

        /// <summary>
        /// Get entity by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id, bool allowTracking = true)
        {
            var data = await Entity.FirstOrDefaultAsync(c =>
            ((int)c.GetType().GetProperty("Id").GetValue(c) == id));

            return data;
        }

        /// <summary>
        /// Get entities by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool allowTracking = true)
        {
            T data;

            if (allowTracking)
            {
                data = await Entity.FirstOrDefaultAsync(predicate);
            }
            else
            {
                data = await Entity.AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            return data;
        }

        /// <summary>
        /// Get entities from sql string async
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FromSqlQueryAsync(string sql, bool allowTracking = true)
        {
            IEnumerable<T> data;

            if (allowTracking)
            {
                data = await Entity.FromSql(sql).ToListAsync();
            }
            else
            {
                data = await Entity.AsNoTracking().FromSql(sql).ToListAsync();
            }
            return data;
        }

        /// <summary>
        /// Get paging async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="allowTracking"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetPagingAsync(Expression<Func<T, bool>> predicate, int page, int pageSize, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return await Entity.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return await Entity.AsNoTracking().Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
