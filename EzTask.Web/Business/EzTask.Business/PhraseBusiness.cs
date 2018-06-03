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
    public class PhraseBusiness : BaseBusiness<EzTaskDbContext>
    {
        private readonly IRepository<Phrase> _phraseRepository;

        public PhraseBusiness(
            IRepository<Phrase> phraseRepository)
        {
            _phraseRepository = phraseRepository;
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
                _phraseRepository.Add(phrase);
            }
            else
            {
                _phraseRepository.Update(phrase);
            }

            var iResult = await UnitOfWork.CommitAsync();

            if(iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = phrase.ToModel();
            }
            return result;
        }

        public async Task<IEnumerable<PhraseModel>> GetPhrases(int projectId)
        {
            var data = await _phraseRepository.GetManyAsync(c => c.ProjectId == projectId, allowTracking: false);

            return data.ToModels();
            //TODO count task item in phrase
        }
    }
}
