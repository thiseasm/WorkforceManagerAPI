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

        public GenericResult<List<Employee>> GetAll()
        {
            var result = new GenericResult<List<Employee>>();
            try
            {
                result.Data = _workforceDbContext.Employees.Where(e => !e.IsDeleted).OrderBy(e => e.Surname).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public GenericResult<Employee> GetEmployeeById(int id)
        {
            var result = new GenericResult<Employee>();
            try
            {
                result.Data = _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public GenericResult<List<Employee>> GetEmployeesBySearchTerm(string term)
        {
            var result = new GenericResult<List<Employee>>();
            try
            {
                result.Data = _workforceDbContext.Employees.Where(e => e.Name.Contains(term) || e.Surname.Contains(term)).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result RemoveEmployee(int id)
        {
            var result = new Result();
            var employee = _workforceDbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null || employee.IsDeleted)
            {
                result.Message = "NotFound";
                return result;
            }
                

            employee.IsDeleted = true;

            try
            {
                _workforceDbContext.Employees.Update(employee);
                _workforceDbContext.SaveChanges();
                result.Success = true;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result SaveEmployee(Employee employee)
        {
            var result = new Result();
            try
            {
                if (employee.Id == 0)
                    _workforceDbContext.Employees.Add(employee);
                else
                    _workforceDbContext.Employees.Update(employee);
            
                _workforceDbContext.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result MassRemoveEmployees(List<int> ids)
        {
            var result = new Result();
            var employeesToBeDeleted = _workforceDbContext.Employees.Where(e => ids.Contains(e.Id));
            foreach (var employee in employeesToBeDeleted)
            {
                employee.IsDeleted = true;
            }

            try
            {
                _workforceDbContext.Employees.UpdateRange(employeesToBeDeleted);
                _workforceDbContext.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
