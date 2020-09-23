using System.Collections.Generic;
using Database.DbModels;

namespace Repository.Interfaces
{
    public interface ISkillRepository
    {
        List<Skill> GetAll();
        Skill GetSkillById(int id);
        void SaveOrUpdate(Skill skill);
        void Delete(Skill skill);
    }
}
