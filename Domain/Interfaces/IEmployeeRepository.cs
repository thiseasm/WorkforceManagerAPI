using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        GenericResult<List<Employee>> GetAll();
        GenericResult<Employee> GetEmployeeById(int id);
        GenericResult<List<Employee>> GetEmployeesBySearchTerm(string term);
        Result RemoveEmployee(int id);
        Result SaveEmployee(Employee employee);
        Result MassRemoveEmployees(List<int> ids);
    }
}
