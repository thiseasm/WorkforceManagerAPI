using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISkillRepository
    {
        Result<List<Skill>> GetAll();
        Result<Skill> GetSkillById(int id);
        void SaveSkill(Skill skill);
        void RemoveSkill(int id);
        void MassRemoveSkills(List<int> ids);
    }
}
