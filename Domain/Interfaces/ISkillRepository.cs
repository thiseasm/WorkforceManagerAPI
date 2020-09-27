using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISkillRepository
    {
        List<Skill> GetAll();
        Skill GetSkillById(int id);
        void SaveSkill(Skill skill);
        void RemoveSkill(int id);
        void MassRemoveSkills(List<int> ids);
    }
}
