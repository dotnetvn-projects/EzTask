using EzTask.Entity.Data;
using EzTask.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using EzTask.Repository;
using EzTask.DataAccess;

namespace EzTask.Repository
{
    public class ProjectRepository : Repository<Project>
    {
        protected ProjectRepository(EzTaskDbContext dataContext) : base(dataContext)
        {
        }
    }
}
