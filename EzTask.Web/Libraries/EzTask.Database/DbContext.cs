using Microsoft.EntityFrameworkCore;

namespace EzTask.Database
{
    public class DbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        private EntityMapper _entityMapper;
        public DbContext(DbContextOptions<DbContext> options)
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
