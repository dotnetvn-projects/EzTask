using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.Entity.Data;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class SkillBusiness : BusinessCore
    {
        public SkillBusiness(UnitOfWork unitOfWork):base(unitOfWork)
        {

        }

        /// <summary>
        /// Save account - skill
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Skill>> SaveAccountSkill(string skill, int accountId)
        {
            using (var transaction = UnitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    //Delete old skills first
                    await DeleteAccountSkill(accountId);

                    //Process adding new skills
                    List<Skill> skills = new List<Skill>();
                    var skillArray = skill.Split(",");

                    foreach (var item in skillArray)
                    {
                        AccountSkill accountSkill = new AccountSkill
                        {
                            AccountId = accountId
                        };

                        string skillName = item.Trim();
                        var existItem = await GetSkill(skillName);

                        if (existItem != null)
                        {
                            accountSkill.SkillId = existItem.Id;
                        }
                        else
                        {
                            var newSkill = new Skill
                            {
                                SkillName = skillName
                            };

                            UnitOfWork.SkillRepository.Add(newSkill);                         
                            await UnitOfWork.CommitAsync();

                            skills.Add(newSkill);

                            accountSkill.SkillId = newSkill.Id;
                        }

                        UnitOfWork.AccountSkillRepository.Add(accountSkill);
                    }

                    var iResult = await UnitOfWork.CommitAsync();
                    transaction.Commit();

                    if (iResult > 0)
                    {
                        return skills;
                    }
                    return null;
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }               
        }

        /// <summary>
        /// Get skill by skill name
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public async Task<Skill> GetSkill(string skill)
        {
            return await UnitOfWork.SkillRepository.GetAsync(c => 
                    c.SkillName.ToLower() == skill.ToLower(), allowTracking: false);
        }

        /// <summary>
        /// Get skill for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>string</returns>
        public async Task<string> GetSkill(int accountId)
        {
            var data = await UnitOfWork.AccountSkillRepository.Entity.AsNoTracking()
                .Include(c => c.Skill)
                .Where(c => c.AccountId == accountId)
                .Select(c=>c.Skill.SkillName)
                .AsNoTracking()
                .ToListAsync();

            if(data.Any())
            {
                return String.Join(", ", data);
            }
            return string.Empty;
        }

        #region Private
        private async Task DeleteAccountSkill(int accountId)
        {
            var skills = await UnitOfWork
                .AccountSkillRepository
                .GetManyAsync(c => c.AccountId == accountId, allowTracking: false);

            if(skills.Any())
            {
                UnitOfWork.AccountSkillRepository.DeleteRange(skills);
                await UnitOfWork.CommitAsync();
            }
        }
        #endregion
    }
}
