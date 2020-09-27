using System.Collections.Generic;
using Database.DbModels;

namespace Repository.Interfaces
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
