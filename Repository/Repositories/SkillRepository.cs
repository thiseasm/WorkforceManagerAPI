using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public SkillRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext;
        }

        public List<Skill> GetAll()
        {
            return _workforceDbContext.Skills.OrderBy(s => s.Name).ToList();
        }

        public Skill GetSkillById(int id)
        {
            return _workforceDbContext.Skills.FirstOrDefault(s => s.Id == id);
        }

        public void SaveOrUpdate(Skill skill)
        {
            if(skill.Id == 0)
                _workforceDbContext.Skills.Update(skill);
            else
             _workforceDbContext.Skills.Add(skill);
            
            _workforceDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var skillToBeRemoved = _workforceDbContext.Skills.Find(id);
            _workforceDbContext.Skills.Remove(skillToBeRemoved);
            _workforceDbContext.SaveChanges();
        }

        public void MassDelete(List<int> ids)
        {
            var skillsToBeRemoved = _workforceDbContext.Skills.Where(s => ids.Contains(s.Id));
            _workforceDbContext.RemoveRange(skillsToBeRemoved);
        }
    }
}
