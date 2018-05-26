using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Framework;
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

        /// <summary>
        /// Save a phrase
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel> Save(PhraseModel model)
        {
            ResultModel result = new ResultModel();
           
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

            if(iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Value = phrase.ToModel();
            }
            else
            {
                result.Status = ActionStatus.Failed;
            }
            return result;
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
