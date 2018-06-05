using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Interfaces;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class PhraseBusiness : BusinessCore
    {
        public PhraseBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save a phrase
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel<PhraseModel>> Save(PhraseModel model)
        {
            ResultModel<PhraseModel> result = new ResultModel<PhraseModel>();

            var phrase = model.ToEntity();
            if (phrase.Id < 1)
            {
                UnitOfWork.PhraseRepository.Add(phrase);
            }
            else
            {
                UnitOfWork.PhraseRepository.Update(phrase);
            }

            var iResult = await UnitOfWork.CommitAsync();

            if(iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = phrase.ToModel();
            }
            return result;
        }

        public async Task<IEnumerable<PhraseModel>> GetPhrase(int projectId)
        {
            var data = await UnitOfWork.PhraseRepository.GetManyAsync(c => c.ProjectId == projectId, allowTracking: false);
            return data.ToModels();
            //TODO count task item in phrase
        }

        public async Task<PhraseModel> GetOpenFeaturePhrase(int projectId)
        {
            var data = await UnitOfWork.PhraseRepository.GetAsync(c => 
            c.ProjectId == projectId && c.PhraseName == "Open Features",
                allowTracking: false);

            return data.ToModel();
            //TODO count task item in phrase
        }
    }
}
