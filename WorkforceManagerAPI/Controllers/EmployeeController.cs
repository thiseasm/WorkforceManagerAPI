using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WorkforceManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHistoryRepository _historyRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IHistoryRepository historyRepository)
        {
            _employeeRepository = employeeRepository;
            _historyRepository = historyRepository;
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
        public ActionResult<Employee> GetEmployee(int id)
        {
            var getResult = _employeeRepository.GetEmployeeById(id);

            if (getResult.Success)
            {
                return getResult.Data;
            }

            return NotFound();
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
            var getEmployeeResult = _employeeRepository.GetEmployeeById(employee.Id);
            if (!getEmployeeResult.Success)
            {
                return getEmployeeResult;
            }

            return getEmployeeResult.Data != null 
                ? RegisterEmployee(employee)
                : UpdateEmployee(employee,getEmployeeResult.Data);
        }

        private Result UpdateEmployee(Employee employee,Employee registered)
        {
            var result = new Result();
            var removedEntry = GenerateRemovedEntry(employee, registered);
            var addedEntry = GeneratedAddedEntry(employee, registered);

            var saveResult = _employeeRepository.SaveEmployee(employee);
            if (!saveResult.Success)
            {
                result.Message ="SaveEmployeeFailed";
                return result;
            }

            var entries = new List<HistoryEntry>{addedEntry,removedEntry};
            var saveHistoryResult = _historyRepository.LogEntries(entries);
            if (!saveHistoryResult.Success)
            {
                result.Message = "SaveHistoryFailed";
                return result;
            }

            result.Success = true;
            return result;
        }

        private Result RegisterEmployee(Employee employee)
        {
            var result = new Result();
            var saveResult = _employeeRepository.SaveEmployee(employee);
            if (!saveResult.Success)
            {
                result.Message ="SaveEmployeeFailed";
                return result;
            }
            

            if (employee.Skillset == null || !employee.Skillset.Any())
            {
                result.Success = true;
                return result;
            }

            var entry = GeneratedAddedEntry(employee);
            var saveHistoryResult = _historyRepository.LogEntry(entry);
            if (!saveHistoryResult.Success)
            {
                result.Message = "SaveHistoryFailed";
                return result;
            }

            result.Success = true;
            return result;
        }

        private static HistoryEntry GeneratedAddedEntry(Employee employee)
        {
            return new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee,
                ChangedSkills = employee.Skillset
            };
        }

        private static HistoryEntry GeneratedAddedEntry(Employee employee, Employee registeredEmployee)
        {
            var skillsAdded = employee.Skillset.Where(e => !registeredEmployee.Skillset.Select(s => s.Id).Contains(e.Id))
                .ToList();

            return new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee,
                ChangedSkills = skillsAdded
            };
        }

        private static HistoryEntry GenerateRemovedEntry(Employee employee, Employee registeredEmployee)
        {
            var skillsRemoved = registeredEmployee.Skillset.Where(e => !employee.Skillset.Select(s => s.Id).Contains(e.Id))
                .ToList();

            return new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Removed",
                Target = employee,
                ChangedSkills = skillsRemoved
            };
        }
    }
}