using Microsoft.EntityFrameworkCore;

namespace EzTask.Database
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
