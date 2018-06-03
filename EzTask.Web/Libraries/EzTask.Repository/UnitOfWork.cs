using EzTask.DataAccess;
using EzTask.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzTask.Repository
{
    public class UnitOfWork : IUnitOfWork<EzTaskDbContext>
    {
        public EzTaskDbContext Context { get; }

        public UnitOfWork(EzTaskDbContext context)
        {
            Context = context;
        }

        public int Commit()
        {
            var iResult = Context.SaveChanges();
            return iResult;
        }

        public async Task<int> CommitAsync()
        {
            var iResult = await Context.SaveChangesAsync();
            return iResult;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
