using System;
using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

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
            return _employeeRepository.GetAll();
        }

        // GET: api/Employee/id
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);

            if (employee == null)
                return NotFound();
            
            return employee;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult<Employee> SaveEmployee(Employee employee)
        {
            LogHistoryAndSave(employee);
            return CreatedAtAction("GetEmployees",null);
        }

        // DELETE: api/Employee/id
        [HttpDelete("{id}")]
        public ActionResult<Employee> DeleteEmployee(int id)
        {
            _employeeRepository.DeleteEmployee(id);
            return Ok();
        }

        // DELETE: api/Employee
        [HttpDelete]
        [Route("MassDelete")]
        public ActionResult<Employee> MassDelete(List<int> ids)
        {
            _employeeRepository.MassDelete(ids);
            return Ok();
        }

        private void LogHistoryAndSave(Employee employee)
        {
            var registeredEmployee = _employeeRepository.GetEmployeeById(employee.Id);
            if (registeredEmployee == null)
                RegisterEmployee(employee);
            else
                UpdateEmployee(employee,registeredEmployee);
        }

        private void UpdateEmployee(Employee employee,Employee registered)
        {
            var removedEntry = GenerateRemovedEntry(employee, registered);
            var addedEntry = GeneratedAddedEntry(employee, registered);

            _employeeRepository.SaveOrUpdate(employee);
            _historyRepository.LogEntry(addedEntry);
            _historyRepository.LogEntry(removedEntry);
        }

        private void RegisterEmployee(Employee employee)
        {
            _employeeRepository.SaveOrUpdate(employee);
            if(employee.Skillset == null || !employee.Skillset.Any())
                return;

            var entry = new HistoryEntry
            {
                Date = DateTime.Now,
                Description = "Added",
                Target = employee,
                ChangedSkills = employee.Skillset
            };
            _historyRepository.LogEntry(entry);
        }

        private static HistoryEntry GeneratedAddedEntry(Employee employee, Employee registeredEmployee)
        {
            var skillsAdded = employee.Skillset.Where(e => !registeredEmployee.Skillset.Select(s => s.Id).Contains(e.Id))
                .ToList();
            return new HistoryEntry
            {
                Date = DateTime.Now,
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
                Date = DateTime.Now,
                Description = "Removed",
                Target = employee,
                ChangedSkills = skillsRemoved
            };
        }
    }
}