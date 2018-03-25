using EzTask.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace EzTask.DataAccess
{
    public class EzTaskDbContext:DbContext
    {
        public EzTaskDbContext(DbContextOptions<EzTaskDbContext> options)
        : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountInfo> AccountInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => new EntityMapper(modelBuilder).Map();
    }
}
