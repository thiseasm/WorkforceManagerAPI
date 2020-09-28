using System.Collections.Generic;
using Domain.Models;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        GenericResult<List<Employee>> GetAll();
        GenericResult<Employee> GetEmployeeById(int id);
        GenericResult<List<HistoryEntry>> GetHistoryEntriesForEmployee(int employeeId);
        GenericResult<List<Employee>> GetEmployeesBySearchTerm(string term);
        Result RemoveEmployee(int id);
        Result LogHistoryAndSave(Employee employee);
        Result MassRemoveEmployees(List<int> ids);
    }
}
