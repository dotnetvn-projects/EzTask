using EzTask.Entity;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.DataAccess
{
    public class EzTaskDbContext:DbContext
    {
        private EntityMapper _entityMapper;
        public EzTaskDbContext(DbContextOptions<EzTaskDbContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _entityMapper = new EntityMapper(modelBuilder);
            _entityMapper.Map();
        }
    }
}
