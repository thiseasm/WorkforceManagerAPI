using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WorkforceManagerAPI.ViewModels;

namespace WorkforceManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly ISkillRepository _skillRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IHistoryRepository historyRepository, ISkillRepository skillRepository)
        {
            _employeeRepository = employeeRepository;
            _historyRepository = historyRepository;
            _skillRepository = skillRepository;
        }

        // GET: api/Employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            var getResult = _employeeRepository.GetAll();
            if (getResult.Success)
            {
                return getResult.Data;
            }

            return NotFound();

        }

        // GET: api/Employee/id
        [HttpGet("{id}")]
        public ActionResult<EmployeeViewModel> GetEmployee(int id)
        {
            var getResult = _employeeRepository.GetEmployeeById(id);

            if (!getResult.Success)
            {
                return NotFound();;
            }

            var employee = getResult.Data;

            var employeeViewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                HiredAt = employee.HiredAt.ToString("d"),
                Skills = new List<SkillViewModel>()
            };

            if (employee.EmployeeSkillset.Select(s => s.Skill).Any())
            {
                employeeViewModel.Skills = employee.EmployeeSkillset.Select(s => s.Skill).Select(s => new SkillViewModel
                {
                    CreatedAt = s.CreatedAt.ToString("d"),
                    Description = s.Description,
                    Id = s.Id,
                    Title = s.Title

                })
                .ToList();
            }

            return employeeViewModel;
        }

        // GET: api/Employee/id/History
        [HttpGet("{id}/History")]
        public  ActionResult<IEnumerable<HistoryEntry>> GetHistoryForEmployee(int id)
        {
            var employeeHistoryResult = _historyRepository.GetEntriesForEmployee(id);

            if (!employeeHistoryResult.Success)
                return NotFound();
            
            return employeeHistoryResult.Data;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult<Employee> SaveEmployee(Employee employee)
        {
            var saveEmployeeResult = LogHistoryAndSave(employee);
            if (!saveEmployeeResult.Success)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Employee/id
        [HttpDelete("{id}")]
        public ActionResult<Employee> DeleteEmployee(int id)
        {
            var removeResult = _employeeRepository.RemoveEmployee(id);

            if (removeResult.Success)
                return Ok();

            if(removeResult.Message.Equals("NotFound"))
                return NotFound();

            return BadRequest();

        }

        // DELETE: api/Employee
        [HttpDelete]
        [Route("MassRemoveEmployees")]
        public ActionResult<Employee> MassDelete(List<int> ids)
        {
            var removeResult = _employeeRepository.MassRemoveEmployees(ids);
            if(removeResult.Success)
                return Ok();

            return BadRequest();
        }

        private Result LogHistoryAndSave(Employee employee)
        {
            var getEmployeeResult = _employeeRepository.GetEmployeeByIdWithoutSkills(employee.Id);
            if (!getEmployeeResult.Success)
            {
                return getEmployeeResult;
            }

            return (getEmployeeResult.Data == null && employee.Id == 0)
                ? RegisterEmployee(employee)
                : UpdateEmployee(employee,getEmployeeResult.Data);
        }

        private Result UpdateEmployee(Employee employee,Employee registered)
        {
            var result = new Result();
            var employeeSkills = registered.EmployeeSkillset ?? new List<EmployeeSkill>();
            var skillsToRemove = new List<EmployeeSkill>();
            registered.History ??= new List<HistoryEntry>();

            if (employee.SkillIds != null && employee.SkillIds.Any())
            {
                var getSkillsResult = _skillRepository.GetSkillsById(employee.SkillIds?.ToList());
                if (!getSkillsResult.Success)
                    return result;

                skillsToRemove = employeeSkills.Where(s => !employee.SkillIds.Contains(s.SkillId)).ToList();

                foreach (var skillAdded in getSkillsResult.Data.Select(skill => new EmployeeSkill
                    {Employee = employee, Skill = skill, SkillId = skill.Id}))
                {
                    employeeSkills.Add(skillAdded);
                }

                var removedEntry = GenerateRemovedEntry(employee, registered);
                var addedEntry = GeneratedAddedEntry(employee, registered);

                registered.History.Add(removedEntry);
                registered.History.Add(addedEntry);
            }
            else
            {
                skillsToRemove = employeeSkills.ToList();

                if (registered.EmployeeSkillset != null && registered.EmployeeSkillset.Any())
                {
                    var removedEntry = GenerateRemovedEntry(employee, registered);
                    registered.History.Add(removedEntry);
                }
            }

            employee.EmployeeSkillset = employeeSkills;
            employee.History = registered.History;

            var saveResult = _employeeRepository.SaveEmployee(employee,skillsToRemove);
            if (!saveResult.Success)
            {
                result.Message ="SaveEmployeeFailed";
                return result;
            }

            result.Success = true;
            return result;
        }

        private Result RegisterEmployee(Employee employee)
        {
            var result = new Result();
            var employeeSkills = new List<EmployeeSkill>();
            var employeeHistory = new List<HistoryEntry>();

            if (employee.SkillIds != null && employee.SkillIds.Any())
            {
                var getSkillsResult = _skillRepository.GetSkillsById(employee.SkillIds?.ToList());
                if (!getSkillsResult.Success)
                    return result;

                employeeSkills.AddRange(getSkillsResult.Data.Select(skill => new EmployeeSkill {Employee = employee, Skill = skill, SkillId = skill.Id}));

                var historyEntry = GeneratedAddedEntry(employee, getSkillsResult.Data);
                employee.EmployeeSkillset = employeeSkills;
                employeeHistory.Add(historyEntry);
                employee.History = employeeHistory;
            }

            var saveResult = _employeeRepository.SaveEmployee(employee);
            if (!saveResult.Success)
            {
                result.Message ="SaveEmployeeFailed";
                return result;
            }

            result.Success = true;
            return result;
        }

        private static HistoryEntry GeneratedAddedEntry(Employee employee, IEnumerable<Skill> addedSkills)
        {
            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee
            };

            var changedSkills = addedSkills.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();
            historyEntry.ChangedSkills = changedSkills;
            return historyEntry;
        }

        private HistoryEntry GeneratedAddedEntry(Employee employee, Employee registeredEmployee)
        {
            var skillIdsAdded = employee.SkillIds
                                        .Where(id => !registeredEmployee.EmployeeSkillset
                                                .Select(s => s.SkillId)
                                                .Contains(id))
                                        .ToList();

            var getSkillsAddedResult = _skillRepository.GetSkillsById(skillIdsAdded);
            if (!getSkillsAddedResult.Success)
                return null;

            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee,
            };
            var addedSkills = getSkillsAddedResult.Data.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();

            historyEntry.ChangedSkills = addedSkills;
            return historyEntry;
        }

        private HistoryEntry GenerateRemovedEntry(Employee employee, Employee registeredEmployee)
        {
            List<int> skillIdsRemoved;

            if (employee.SkillIds == null)
            {
                skillIdsRemoved = registeredEmployee.EmployeeSkillset.Select(s => s.SkillId).ToList();
            }
            else
            {
                skillIdsRemoved = registeredEmployee.EmployeeSkillset
                                    .Select(es => es.SkillId)
                                    .Where(e => !employee.SkillIds.Contains(e))
                                    .ToList();
            }
                
                

            var getSkillsRemovedResult = _skillRepository.GetSkillsById(skillIdsRemoved);
            if (!getSkillsRemovedResult.Success)
                return null;

            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Removed",
                Target = employee
            };

            var removedSkills = getSkillsRemovedResult.Data.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();

            historyEntry.ChangedSkills = removedSkills;
            return historyEntry;
        }
    }
}