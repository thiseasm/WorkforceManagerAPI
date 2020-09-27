using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetEmployeeById(int id);
        List<Employee> GetEmployeesBySearchTerm(string term);
        void RemoveEmployee(int id);
        void SaveEmployee(Employee employee);
        void MassRemoveEmployees(List<int> ids);
    }
}
