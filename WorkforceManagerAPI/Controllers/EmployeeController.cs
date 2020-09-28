using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WorkforceManagerAPI.ViewModels;

namespace WorkforceManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            var getResult = _employeeService.GetAll();
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
            var getResult = _employeeService.GetEmployeeById(id);

            if (!getResult.Success)
            {
                return NotFound();
            }

            var employee = getResult.Data;
            var employeeViewModel = new EmployeeViewModel(employee);
            var employeeSkills = employee.EmployeeSkillset.Select(s => s.Skill).ToList();

            if (employeeSkills.Any())
            {
                foreach (var skill in employeeSkills)
                {
                    employeeViewModel.Skills.Add(new SkillViewModel(skill));
                }
            }

            return employeeViewModel;
        }

        // GET: api/Employee/id/History
        [HttpGet("{id}/History")]
        public  ActionResult<IEnumerable<HistoryEntry>> GetHistoryForEmployee(int id)
        {
            var employeeHistoryResult = _employeeService.GetHistoryEntriesForEmployee(id);

            if (!employeeHistoryResult.Success)
                return NotFound();
            
            return employeeHistoryResult.Data;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult<Employee> SaveEmployee(Employee employee)
        {
            var saveEmployeeResult = _employeeService.LogHistoryAndSave(employee);
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
            var removeResult = _employeeService.RemoveEmployee(id);

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
            var removeResult = _employeeService.MassRemoveEmployees(ids);
            if(removeResult.Success)
                return Ok();

            return BadRequest();
        }

    }
}