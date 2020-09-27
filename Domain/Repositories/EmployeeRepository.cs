using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public EmployeeRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext ?? throw new ArgumentNullException();
        }

        public List<Employee> GetAll()
        {
            return _workforceDbContext.Employees.Where(e => !e.IsDeleted).OrderBy(e => e.Surname).ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
        }

        public List<Employee> GetEmployeesBySearchTerm(string term)
        {
            return _workforceDbContext.Employees.Where(e => e.Name.Contains(term) || e.Surname.Contains(term)).ToList();
        }

        public void RemoveEmployee(int id)
        {
            var employee = _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id);
            if(employee == null || employee.IsDeleted)
                return;

            employee.IsDeleted = true;
            _workforceDbContext.Employees.Update(employee);
            _workforceDbContext.SaveChanges();
        }

        public void SaveEmployee(Employee employee)
        {
            if(employee.Id == 0)
                _workforceDbContext.Employees.Add(employee);
            else
                _workforceDbContext.Employees.Update(employee);
             
            
            _workforceDbContext.SaveChanges();
        }

        public void MassRemoveEmployees(List<int> ids)
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
