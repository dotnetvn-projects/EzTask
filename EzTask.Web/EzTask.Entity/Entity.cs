using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EzTask.Entity
{
    public class Entity<T>
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public
                | BindingFlags.Instance);

            foreach (PropertyInfo p in props)
            {
                if (!p.CanRead || !p.CanWrite) continue;
                object val = entity.GetType().GetProperty(p.Name).GetValue(entity);
                p.SetValue(this, val , null);             
            }
        }

        /// <summary>
        /// Create a copy
        /// </summary>
        /// <returns></returns>
        public virtual T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
