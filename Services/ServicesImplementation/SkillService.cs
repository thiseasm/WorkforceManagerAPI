using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using Services.Interfaces;

namespace Services.ServicesImplementation
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public GenericResult<List<Skill>> GetAll()
        {
            return _skillRepository.GetAll();
        }

        public GenericResult<Skill> GetSkillById(int id)
        {
            return _skillRepository.GetSkillById(id);
        }

        public GenericResult<List<Skill>> GetSkillsById(List<int> ids)
        {
            return _skillRepository.GetSkillsById(ids);
        }

        public Result SaveSkill(Skill skill)
        {
            return _skillRepository.SaveSkill(skill);
        }

        public Result RemoveSkill(int id)
        {
            return _skillRepository.RemoveSkill(id);
        }

        public Result MassRemoveSkills(List<int> ids)
        {
            return _skillRepository.MassRemoveSkills(ids);
        }
    }
}
