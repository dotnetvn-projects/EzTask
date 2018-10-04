using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Models;
using EzTask.Models.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            Phrase phrase = model.ToEntity();
            if (phrase.Id < 1)
            {
                phrase.Status = (short)PhraseStatus.Open;
                UnitOfWork.PhraseRepository.Add(phrase);
            }
            else
            {
                var isDefault = IsDefault(model.Id);
                phrase.IsDefault = isDefault;

                UnitOfWork.PhraseRepository.Update(phrase);
            }

            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = phrase.ToModel();
            }
            return result;
        }

        public async Task<PhraseModel> GetPhraseById(int phraseId)
        {
            Phrase data = await UnitOfWork.PhraseRepository.GetByIdAsync(phraseId);
            return data.ToModel();
        }

        public async Task<int> CountByProject(int projectId)
        {
            var data = await UnitOfWork.PhraseRepository.Entity.CountAsync(c=>c.ProjectId == projectId);
            return data;
        }

        public async Task<IEnumerable<PhraseModel>> GetPhrase(int projectId)
        {
            IEnumerable<Phrase> data = await UnitOfWork.PhraseRepository
                                                        .GetManyAsync(c => c.ProjectId == projectId, allowTracking: false);
            var model = data.ToModels();

            return model;
        }

        public async Task<PhraseModel> GetOpenFeaturePhrase(int projectId)
        {
            Phrase data = await UnitOfWork.PhraseRepository.GetAsync(c =>
            c.ProjectId == projectId && c.IsDefault == true,
                allowTracking: false);

            return data.ToModel();
        }

        public bool IsDefault(int phraseId)
        {
            Phrase data = UnitOfWork.PhraseRepository.Get(c => c.Id == phraseId && c.IsDefault);

            return data != null;
        }

        /// <summary>
        /// Delete phrase
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<PhraseModel>> Delele(PhraseModel model)
        {
            ResultModel<PhraseModel> result = new ResultModel<PhraseModel>();

            UnitOfWork.PhraseRepository.Delete(model.Id);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
            }

            return result;
        }
    }
}
