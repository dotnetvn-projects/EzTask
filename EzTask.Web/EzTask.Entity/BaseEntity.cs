using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EzTask.Entity
{
    public class BaseEntity<T>
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
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public
                | BindingFlags.Instance);

            foreach (PropertyInfo p in props)
            {
                if (!p.CanRead || !p.CanWrite) continue;

                object val = p.GetGetMethod().Invoke(entity, null);
                object defaultVal = p.PropertyType.IsValueType ? 
                    Activator.CreateInstance(p.PropertyType) : null;

                if (null != defaultVal && !val.Equals(defaultVal))
                {
                    p.GetSetMethod().Invoke(this, new[] { val });
                }
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
