using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.Business.Infrastructures;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Interfaces.Models;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class PhraseBusiness : BaseBusiness
    {
        public PhraseBusiness(EzTaskDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPhraseModel> Save(IPhraseModel phraseModel)
        {
            var entity = phraseModel.MapToEntity();
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

            return entity.MapToModel();
        }

        public async Task<IEnumerable<IPhraseModel>> GetPhrases(int projectId)
        {
            var data = await DbContext.Phrases.AsNoTracking()
                .Where(c => c.ProjectId == projectId).ToListAsync();

            return data.MapToModels();
            //TODO count task item in phrase
        }
    }
}
