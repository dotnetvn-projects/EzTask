using Microsoft.EntityFrameworkCore;

namespace EzTask.Database
{
    public class DataContext: DbContext
    {
        private EntityMapper _entityMapper;
        public DataContext(DbContextOptions<DataContext> options)
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
