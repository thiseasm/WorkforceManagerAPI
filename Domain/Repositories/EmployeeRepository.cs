using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public EmployeeRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext;
        }

        public List<Employee> GetAll()
        {
            return _workforceDbContext.Employees.Where(e => !e.IsDeleted).OrderBy(e => e.Surname).ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
        }

        public void DeleteEmployee(int id)
        {
            var employee = _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id);
            if(employee == null || employee.IsDeleted)
                return;

            employee.IsDeleted = true;
            _workforceDbContext.Employees.Update(employee);
            _workforceDbContext.SaveChanges();
        }

        public void SaveOrUpdate(Employee employee)
        {
            if(employee.Id == 0)
                _workforceDbContext.Employees.Update(employee);
            else
             _workforceDbContext.Employees.Add(employee);
            
            _workforceDbContext.SaveChanges();
        }

        public void MassDelete(List<int> ids)
        {
            var employeesToBeDeleted = _workforceDbContext.Employees.Where(e => ids.Contains(e.Id));
            foreach (var employee in employeesToBeDeleted)
            {
                employee.IsDeleted = true;
            }
            _workforceDbContext.Employees.UpdateRange(employeesToBeDeleted);
            _workforceDbContext.SaveChanges();
        }
    }
}
