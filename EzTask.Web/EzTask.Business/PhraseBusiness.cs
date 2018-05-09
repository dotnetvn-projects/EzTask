using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class PhraseBusiness : BaseBusiness
    {
        public PhraseBusiness(EzTaskDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Phrase>> GetPhrases(int projectId)
        {
           return await DbContext.Phrases.Where(c => c.ProjectId == projectId).ToListAsync();
        }
    }
}
