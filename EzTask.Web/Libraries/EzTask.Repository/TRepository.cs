using EzTask.Interface;
using EzTask.Interface.SharedData;
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
        public void DeleteRange(IList<T> entities)
        {
            Entity.RemoveRange(entities);
        }

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("EF 3.0 is not support client evaluation ")]
        public T GetById(int id, bool allowTracking = true)
        {
            return Entity.FirstOrDefault(c => (int)c.GetType().GetProperty("Id").GetValue(c) == id);
        }

        /// <summary>
        /// Get entity by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> predicate, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.FirstOrDefault(predicate);
            }
            else
            {
                return Entity.AsNoTracking().FirstOrDefault(predicate);
            }
        }

        /// <summary>
        /// Get list of entities
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll(bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.ToList();
            }
            else
            {
                return Entity.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Get entites by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<T> GetMany(Expression<Func<T, bool>> predicate, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.Where(predicate).ToList();
            }
            else
            {
                return Entity.Where(predicate).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Get paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="allowTracking"></param>
        /// <returns></returns>
        public IList<T> GetPaging(Expression<Func<T, bool>> predicate,
           Func<T, Object> orderBy, OrderType orderType, int page, int pageSize, bool allowTracking = true)
        {
            if (allowTracking)
            {
                if (orderType == OrderType.ASC)
                {
                    return Entity.Where(predicate).OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }

                return Entity.Where(predicate).OrderByDescending(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                if (orderType == OrderType.ASC)
                {
                    return Entity.Where(predicate).AsNoTracking().OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }

                return Entity.Where(predicate).AsNoTracking().OrderByDescending(orderBy).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// Count entities by lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Entity.Count(predicate);
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
        public IList<T> FromSqlQuery(string sql, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return Entity.FromSqlRaw(sql).ToList();
            }

            return Entity.FromSqlRaw(sql).AsNoTracking().ToList();
        }

        /// <summary>
        /// Get all entities async
        /// </summary>
        /// <returns></returns>
        public async Task<IList<T>> GetAllAsync(bool allowTracking = true)
        {
            if (allowTracking)
            {
                return await Entity.ToListAsync();
            }

            return await Entity.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get entities by lambda expression async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> predicate,
            bool allowTracking = true)
        {
            if (allowTracking)
            {
                return await Entity.Where(predicate).ToListAsync();
            }
            return await Entity.Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Count entities by lambda expression async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.CountAsync(predicate);
        }

        /// <summary>
        /// Get entity by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete("EF 3.0 is not support client evaluation ")]
        public async Task<T> GetByIdAsync(int id, bool allowTracking = true)
        {
            if (allowTracking)
            {
                return await Entity.FirstOrDefaultAsync(c =>
                 (int)c.GetType().GetProperty("Id").GetValue(c) == id);
            }
            else
            {
                return await Entity.AsNoTracking().FirstOrDefaultAsync(c =>
                 (int)c.GetType().GetProperty("Id").GetValue(c) == id);
            }
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
        public async Task<IList<T>> FromSqlQueryAsync(string sql, bool allowTracking = true)
        {
            IList<T> data;

            if (allowTracking)
            {
                data = await Entity.FromSqlRaw(sql).ToListAsync();
            }
            else
            {
                data = await Entity.FromSqlRaw(sql).AsNoTracking().ToListAsync();
            }
            return data;
        }
    }
}
