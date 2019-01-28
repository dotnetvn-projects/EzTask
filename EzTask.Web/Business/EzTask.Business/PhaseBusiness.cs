using EzTask.Entity.Data;
using EzTask.Framework.Infrastructures;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class PhaseBusiness : BusinessCore
    {
        public PhaseBusiness(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Save a phase
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResultModel</returns>
        public async Task<ResultModel<PhaseModel>> Save(PhaseModel model)
        {
            ResultModel<PhaseModel> result = new ResultModel<PhaseModel>();

            Phase phase = model.ToEntity();
            if (phase.Id < 1)
            {
                phase.Status = (short)PhaseStatus.Open;
                UnitOfWork.PhaseRepository.Add(phase);
            }
            else
            {
                bool isDefault = IsDefault(model.Id);
                phase.IsDefault = isDefault;

                UnitOfWork.PhaseRepository.Update(phase);
            }

            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = phase.ToModel();
            }
            return result;
        }

        /// <summary>
        /// Get phase by id
        /// </summary>
        /// <param name="phaseId"></param>
        /// <returns></returns>
        public async Task<PhaseModel> GetPhaseById(int phaseId)
        {
            Phase data = await UnitOfWork.PhaseRepository.GetByIdAsync(phaseId);
            return data.ToModel();
        }

        /// <summary>
        /// Count Amount of phase in project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<int> CountByProject(int projectId)
        {
            int data = await UnitOfWork.
                PhaseRepository.Entity.CountAsync(c => c.ProjectId == projectId);
            return data;
        }

        /// <summary>
        /// Get phrases in project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PhaseModel>> GetPhases(int projectId)
        {
            IEnumerable<Phase> data = await UnitOfWork.
                PhaseRepository.GetManyAsync(c => c.ProjectId == projectId, allowTracking: false);
            IEnumerable<PhaseModel> model = data.ToModels();

            return model;
        }

        /// <summary>
        /// Get only open feature
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<PhaseModel> GetOpenFeaturePhase(int projectId)
        {
            Phase data = await UnitOfWork.
                PhaseRepository.GetAsync(c => c.ProjectId == projectId && c.IsDefault == true, allowTracking: false);

            return data.ToModel();
        }

        /// <summary>
        /// Check whether phase is default or not
        /// </summary>
        /// <param name="phaseId"></param>
        /// <returns></returns>
        public bool IsDefault(int phaseId)
        {
            Phase data = UnitOfWork.PhaseRepository.Get(c => c.Id == phaseId && c.IsDefault);

            return data != null;
        }

        /// <summary>
        /// Delete phase
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<PhaseModel>> Delele(PhaseModel model)
        {
            ResultModel<PhaseModel> result = new ResultModel<PhaseModel>();

            UnitOfWork.PhaseRepository.Delete(model.Id);
            int iResult = await UnitOfWork.CommitAsync();

            if (iResult > 0)
            {
                result.Status = ActionStatus.Ok;
            }

            return result;
        }
    }
}
