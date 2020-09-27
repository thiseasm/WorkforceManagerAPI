using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
            var getResult = _skillRepository.GetAll();
            if (getResult.Success)
            {
                return getResult.Data;
            }

            return NotFound();
        }

        // GET: api/Skill/id
        [HttpGet("{id}")]
        public ActionResult<Skill> GetSkill(int id)
        {

            var getResult = _skillRepository.GetSkillById(id);

            if (getResult.Success)
            {
                return getResult.Data;
            }

            return NotFound();
        }

        // POST: api/Skill
        [HttpPost]
        public ActionResult<Skill> SaveSkill(Skill skill)
        {
            var saveEmployeeResult = _skillRepository.SaveSkill(skill);
            if (!saveEmployeeResult.Success)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Skill/id
        [HttpDelete("{id}")]
        public ActionResult<Skill> DeleteSkill(int id)
        {

            var removeResult = _skillRepository.RemoveSkill(id);
            

            if (removeResult.Success)
                return Ok();

            if(removeResult.Message.Equals("NotFound"))
                return NotFound();

            return BadRequest();
        }

        // DELETE: api/Skill/MassRemoveEmployees
        [HttpDelete]
        [Route("MassRemoveEmployees")]
        public ActionResult<Skill> MassDelete(List<int> ids)
        {
            var removeResult = _skillRepository.MassRemoveSkills(ids);
            if(removeResult.Success)
                return Ok();

            return BadRequest();
        }
    }
}
