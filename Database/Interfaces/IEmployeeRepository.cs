using System.Collections.Generic;
using Database.DbModels;

namespace Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetEmployeeById(int id);
        Employee GetEmployeeBySearchTerm(string term);
        void RemoveEmployee(int id);
        void SaveEmployee(Employee employee);
        void MassRemoveEmployees(List<int> ids);
    }
}
