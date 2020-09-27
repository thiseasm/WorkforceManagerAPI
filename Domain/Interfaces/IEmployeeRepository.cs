using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Result<List<Employee>> GetAll();
        Result<Employee> GetEmployeeById(int id);
        Result<List<Employee>> GetEmployeesBySearchTerm(string term);
        void RemoveEmployee(int id);
        void SaveEmployee(Employee employee);
        void MassRemoveEmployees(List<int> ids);
    }
}
