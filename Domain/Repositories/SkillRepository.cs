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

        public GenericResult<List<Skill>> GetAll()
        {
            var result = new GenericResult<List<Skill>>();
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

        public GenericResult<Skill> GetSkillById(int id)
        {
            var result = new GenericResult<Skill>();
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

        public GenericResult<List<Skill>> GetSkillsById(List<int> ids)
        {
            var result = new GenericResult<List<Skill>>();
            try
            {
                result.Data = _workforceDbContext.Skills.Where(s => ids.Contains(s.Id)).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result SaveSkill(Skill skill)
        {
            var result = new Result();
            skill.CreatedAt = DateTimeOffset.Now;
            try
            {
                if(skill.Id != 0)
                    _workforceDbContext.Skills.Update(skill);
                else
                    _workforceDbContext.Skills.Add(skill);
            
                _workforceDbContext.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result RemoveSkill(int id)
        {
            var result = new Result();
            var skillResult = GetSkillById(id);
            if (!skillResult.Success)
            {
                result.Message = skillResult.Message;
                result.Success = false;
                return result;
            }
            
            if (skillResult.Data == null)
            {
                result.Message = "NotFound";
                result.Success = false;
                return result;
            }

            try
            {
                _workforceDbContext.Skills.Remove(skillResult.Data);
                _workforceDbContext.SaveChanges();
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result MassRemoveSkills(List<int> ids)
        {   var result = new Result();
            try
            {
                var skillsToBeRemoved = _workforceDbContext.Skills.Where(s => ids.Contains(s.Id));
                _workforceDbContext.RemoveRange(skillsToBeRemoved);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
