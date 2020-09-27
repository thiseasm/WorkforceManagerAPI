using System.Collections.Generic;
using Database.DbModels;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace WorkforceManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;

        public SkillController(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        // GET: api/Skill
        [HttpGet]
        public ActionResult<IEnumerable<Skill>> GetSkills()
        {
            return _skillRepository.GetAll();
        }

        // GET: api/Skill/id
        [HttpGet("{id}")]
        public ActionResult<Skill> GetSkill(int id)
        {
            var skill = _skillRepository.GetSkillById(id);

            if (skill == null)
                return NotFound();
            
            return skill;
        }

        // POST: api/Skill
        [HttpPost]
        public ActionResult<Skill> SaveSkill(Skill skill)
        {
            _skillRepository.SaveSkill(skill);
            return CreatedAtAction("GetSkills",null);
        }

        // DELETE: api/Skill/id
        [HttpDelete("{id}")]
        public ActionResult<Skill> DeleteSkill(int id)
        {
            _skillRepository.RemoveSkill(id);
            return Ok();
        }

        // DELETE: api/Skill/MassRemoveEmployees
        [HttpDelete]
        [Route("MassRemoveEmployees")]
        public ActionResult<Skill> MassDelete(List<int> ids)
        {
            _skillRepository.MassRemoveSkills(ids);
            return Ok();
        }
    }
}
