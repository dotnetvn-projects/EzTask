using EzTask.DataAccess;
using EzTask.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace EzTask.MainBusiness
{
    public class EzTaskBusiness : IEzTaskBusiness
    {
        public AccountBusiness Account { get; }

        public EzTaskBusiness(IConfiguration configuration)
        {
            var context = CreateContext(configuration);
            Account = new AccountBusiness(context);
        }

        /// <summary>
        /// Create dbcontext
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public EzTaskDbContext CreateContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EzTaskDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("EzTask"));
            return new EzTaskDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Track message
        /// </summary>
        public void TrackMe()
        {
            Debug.Print("I am EzTask and I am good to work now :)");
        }
    }
}
