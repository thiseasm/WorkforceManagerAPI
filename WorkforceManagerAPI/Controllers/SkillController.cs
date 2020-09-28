using System.Collections.Generic;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WorkforceManagerAPI.ViewModels;

namespace WorkforceManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // GET: api/Skill
        [HttpGet]
        public ActionResult<IEnumerable<Skill>> GetSkills()
        {
            var getResult = _skillService.GetAll();
            if (getResult.Success)
            {
                return getResult.Data;
            }

            return NotFound();
        }

        // GET: api/Skill/id
        [HttpGet("{id}")]
        public ActionResult<SkillViewModel> GetSkill(int id)
        {

            var getResult = _skillService.GetSkillById(id);

            if (!getResult.Success)
            {
                return NotFound();
            }

            var skill = getResult.Data;
            return new SkillViewModel
            {
                Id = skill.Id,
                CreatedAt = skill.CreatedAt.ToString("d"),
                Description = skill.Description,
                Title = skill.Title
            };
            
        }

        // POST: api/Skill
        [HttpPost]
        public ActionResult<Skill> SaveSkill(Skill skill)
        {
            var saveEmployeeResult = _skillService.SaveSkill(skill);
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
            var removeResult = _skillService.RemoveSkill(id);

            if (removeResult.Success)
                return Ok();

            if(removeResult.Message.Equals("NotFound"))
                return NotFound();

            return BadRequest();
        }

        // DELETE: api/Skill/MassRemoveSkills
        [HttpDelete]
        [Route("MassRemoveSkills")]
        public ActionResult<Skill> MassDelete(List<int> ids)
        {
            var removeResult = _skillService.MassRemoveSkills(ids);
            if(removeResult.Success)
                return Ok();

            return BadRequest();
        }
    }
}
