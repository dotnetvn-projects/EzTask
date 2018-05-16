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

        public async Task<Phrase> Save(Phrase phrase)
        {
            if (phrase.Id < 1)
            {
                DbContext.Phrases.Add(phrase);
            }
            else
            {
                DbContext.Attach(phrase);
                DbContext.Entry(phrase).State = EntityState.Modified;
            }

            var iResult = await DbContext.SaveChangesAsync();
            return phrase;
        }

        public async Task<IEnumerable<Phrase>> GetPhrases(int projectId)
        {
            return await DbContext.Phrases.AsNoTracking()
                .Where(c => c.ProjectId == projectId)
                 .ToListAsync();

            //TODO count task item in phrase
        }
    }
}
