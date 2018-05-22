using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class PhraseBusiness : BaseBusiness
    {
        public PhraseBusiness(EzTaskDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PhraseModel> Save(PhraseModel model)
        {
            var phrase = model.ToEntity();
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

            return phrase.ToModel();
        }

        public async Task<IEnumerable<PhraseModel>> GetPhrases(int projectId)
        {
            var data = await DbContext.Phrases.AsNoTracking()
                .Where(c => c.ProjectId == projectId).ToListAsync();

            return data.ToModels();
            //TODO count task item in phrase
        }
    }
}
