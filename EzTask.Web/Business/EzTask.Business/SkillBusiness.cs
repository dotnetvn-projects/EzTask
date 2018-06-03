using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using EzTask.Interfaces;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class SkillBusiness : BaseBusiness<EzTaskDbContext>
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<AccountSkill> _accountSkillRepository;

        public SkillBusiness(
            IRepository<Skill> skillRepository,
            IRepository<AccountSkill> accountSkillRepository)
        {
            _skillRepository = skillRepository;
            _accountSkillRepository = accountSkillRepository;
        }

        /// <summary>
        /// Save account - skill
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Skill>> SaveAccountSkill(string skill, int accountId)
        {
            using (var dbContextTransaction = UnitOfWork.Context.Database.BeginTransaction())
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

                            _skillRepository.Add(newSkill);                         
                            await UnitOfWork.CommitAsync();

                            skills.Add(newSkill);

                            accountSkill.SkillId = newSkill.Id;
                        }

                        _accountSkillRepository.Add(accountSkill);
                    }

                    var iResult = await UnitOfWork.CommitAsync();
                    dbContextTransaction.Commit();

                    if (iResult > 0)
                    {
                        return skills;
                    }
                    return null;
                }
                catch
                {
                    dbContextTransaction.Rollback();
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
            return await _skillRepository.GetAsync(c => 
                    c.SkillName.ToLower() == skill.ToLower(), allowTracking: false);
        }

        /// <summary>
        /// Get skill for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>string</returns>
        public async Task<string> GetSkill(int accountId)
        {
            var data = await _accountSkillRepository.Entity.AsNoTracking()
                .Include(c => c.Skill)
                .Where(c => c.AccountId == accountId)
                .Select(c=>c.Skill.SkillName)
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
            var skills = await _accountSkillRepository.GetManyAsync(c => c.AccountId == accountId);

            if(skills.Any())
            {
                _accountSkillRepository.DeleteRange(skills);
                await UnitOfWork.CommitAsync();
            }
        }
        #endregion
    }
}
