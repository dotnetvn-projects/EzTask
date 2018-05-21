using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class PhraseBusiness : BaseBusiness
    {
        public PhraseBusiness(EzTaskDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PhraseModel> Save(PhraseModel phraseModel)
        {
            var entity = phraseModel.ToEntity();
            if (entity.Id < 1)
            {
                DbContext.Phrases.Add(entity);
            }
            else
            {
                DbContext.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            var iResult = await DbContext.SaveChangesAsync();

            return entity.ToModel();
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
