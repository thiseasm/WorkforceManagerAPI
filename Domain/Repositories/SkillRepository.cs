using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public SkillRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext ?? throw new ArgumentNullException();
        }

        public Result<List<Skill>> GetAll()
        {
            var result = new Result<List<Skill>>();
            try
            {
                result.Data = _workforceDbContext.Skills.OrderBy(s => s.Title).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<Skill> GetSkillById(int id)
        {
            var result = new Result<Skill>();
            try
            {
                result.Data = _workforceDbContext.Skills.FirstOrDefault(s => s.Id == id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public void SaveSkill(Skill skill)
        {
            if(skill.Id == 0)
                _workforceDbContext.Skills.Update(skill);
            else
             _workforceDbContext.Skills.Add(skill);
            
            _workforceDbContext.SaveChanges();
        }

        public void RemoveSkill(int id)
        {
            var skillToBeRemoved = _workforceDbContext.Skills.Find(id);
            _workforceDbContext.Skills.Remove(skillToBeRemoved);
            _workforceDbContext.SaveChanges();
        }

        public void MassRemoveSkills(List<int> ids)
        {
            var skillsToBeRemoved = _workforceDbContext.Skills.Where(s => ids.Contains(s.Id));
            _workforceDbContext.RemoveRange(skillsToBeRemoved);
        }
    }
}
