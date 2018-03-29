using EzTask.Entity;
using Microsoft.EntityFrameworkCore;
namespace EzTask.DataAccess
{
    internal class EntityMapper
    {
        private readonly ModelBuilder _modelBuilder ;
        public EntityMapper(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Map()
        {
            AccountMap();
        }

        private void AccountMap()
        {
            _modelBuilder.Entity<Account>().ToTable(TableName.Account.ToString());
            _modelBuilder.Entity<AccountInfo>().ToTable(TableName.AccountInfo.ToString());
        }
    }
}
