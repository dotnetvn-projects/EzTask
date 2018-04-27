using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzTask.DataAccess;
using EzTask.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace EzTask.Business
{
    public class SkillBusiness : BaseBusiness
    {
        public SkillBusiness(EzTaskDbContext ezTaskDbContext) :
            base(ezTaskDbContext)
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
            using (var dbContextTransaction = DbContext.Database.BeginTransaction())
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

                            DbContext.Skills.Add(newSkill);                         
                            await DbContext.SaveChangesAsync();

                            skills.Add(newSkill);

                            accountSkill.SkillId = newSkill.Id;
                        }

                        DbContext.AccountSkills.Add(accountSkill);
                    }

                    var iResult = await DbContext.SaveChangesAsync();
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
            return await DbContext.Skills.AsNoTracking()
                .FirstOrDefaultAsync(c => c.SkillName.ToLower() == skill.ToLower());
        }

        /// <summary>
        /// Get skill for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>string</returns>
        public async Task<string> GetSkill(int accountId)
        {
            var data = await DbContext.AccountSkills.AsNoTracking()
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
            var skills = await DbContext.AccountSkills.
                Where(c => c.AccountId == accountId).ToListAsync();

            if(skills.Any())
            {
                DbContext.AccountSkills.RemoveRange(skills);
                await DbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}
