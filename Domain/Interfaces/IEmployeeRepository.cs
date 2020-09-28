using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        GenericResult<List<Employee>> GetAll();
        GenericResult<Employee> GetEmployeeById(int id);
        GenericResult<Employee> GetEmployeeByIdWithoutSkills(int id);
        GenericResult<List<Employee>> GetEmployeesBySearchTerm(string term);
        Result RemoveEmployee(int id);
        Result SaveEmployee(Employee employee);
        Result SaveEmployee(Employee employee,IList<EmployeeSkill> skillsToRemove);
        Result MassRemoveEmployees(List<int> ids);
    }
}
