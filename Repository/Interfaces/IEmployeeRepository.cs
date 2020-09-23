using System.Collections.Generic;
using Database.DbModels;

namespace Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetEmployeeById(int id);
        void DeleteEmployee(int id);
        void SaveOrUpdate(Employee employee);
        void MassDelete(List<int> ids);
    }
}
