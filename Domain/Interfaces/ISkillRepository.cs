using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISkillRepository
    {
        GenericResult<List<Skill>> GetAll();
        GenericResult<Skill> GetSkillById(int id);
        Result SaveSkill(Skill skill);
        Result RemoveSkill(int id);
        Result MassRemoveSkills(List<int> ids);
    }
}
