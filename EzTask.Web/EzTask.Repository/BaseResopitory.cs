using EzTask.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Repository
{
    public abstract class BaseResopitory<T> where T : class
    {
        protected EzTaskDbContext DataContext;
        protected readonly DbSet<T> Dbset;
        protected BaseResopitory(EzTaskDbContext dataContext)
        {
            DataContext = dataContext;
            Dbset = DataContext.Set<T>();
        }
    }
}
