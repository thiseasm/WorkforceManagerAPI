﻿using System.Collections.Generic;
using Domain.Models;

namespace Services.Interfaces
{
    public interface ISkillService
    {
        GenericResult<List<Skill>> GetAll();
        GenericResult<Skill> GetSkillById(int id);
        GenericResult<List<Skill>> GetSkillsById(List<int> ids);
        Result SaveSkill(Skill skill);
        Result RemoveSkill(int id);
        Result MassRemoveSkills(List<int> ids);
    }
}
