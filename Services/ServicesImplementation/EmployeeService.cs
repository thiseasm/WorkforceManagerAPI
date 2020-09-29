using System;
using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;
using Services.Interfaces;

namespace Services.ServicesImplementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeSkillRepository _employeeSkillRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly ISkillRepository _skillRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IHistoryRepository historyRepository, ISkillRepository skillRepository,IEmployeeSkillRepository employeeSkillRepository)
        {
            _employeeSkillRepository = employeeSkillRepository;
            _employeeRepository = employeeRepository;
            _historyRepository = historyRepository;
            _skillRepository = skillRepository;
        }
        public GenericResult<List<Employee>> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public GenericResult<Employee> GetEmployeeById(int id)
        {
            return _employeeRepository.GetEmployeeById(id);
        }

        public GenericResult<List<HistoryEntry>> GetHistoryEntriesForEmployee(int employeeId)
        {
            return _historyRepository.GetEntriesForEmployee(employeeId);
        }

        public GenericResult<List<Employee>> GetEmployeesBySearchTerm(string term)
        {
            return _employeeRepository.GetEmployeesBySearchTerm(term);
        }

        public Result RemoveEmployee(int id)
        {
            return _employeeRepository.RemoveEmployee(id);
        }

        public Result MassRemoveEmployees(List<int> ids)
        {
            return _employeeRepository.MassRemoveEmployees(ids);
        }

        public Result LogHistoryAndSave(Employee employee)
        {
            return employee.Id == 0 ? RegisterEmployee(employee) : UpdateEmployee(employee);
        }

        private Result UpdateEmployee(Employee employee)
        {
            var result = new Result();
            var getEmployeeResult = _employeeRepository.GetEmployeeByIdWithoutSkills(employee.Id);
            if (!getEmployeeResult.Success)
            {
                result.Message = "NotFound";
                return result;
            }

            var registeredEmployee = getEmployeeResult.Data;

            if (employee.SkillIds == null || !employee.SkillIds.Any())
            {
                return ClearSkillsetAndSave(registeredEmployee);
            }

            return UpdateSkillsetAndSave(employee, registeredEmployee);
        }

        private Result UpdateSkillsetAndSave(Employee employee, Employee registeredEmployee)
        {
            RemoveEmployeeSkills(employee, registeredEmployee);
            AddEmployeeSkills(employee, registeredEmployee);

            _employeeRepository.SaveEmployee(registeredEmployee);
            return _employeeRepository.SaveChanges();
        }

        private void AddEmployeeSkills(Employee employee, Employee registeredEmployee)
        {
            var skillIdsToAdd = employee.SkillIds
                .Where(si => !registeredEmployee.EmployeeSkillset.Select(es => es.SkillId).Contains(si)).ToList();
            foreach (var skillId in skillIdsToAdd)
            {
                registeredEmployee.EmployeeSkillset.Add(new EmployeeSkill
                {
                    Employee = registeredEmployee,
                    SkillId = skillId
                });
            }

            _employeeSkillRepository.AddEntries(registeredEmployee.EmployeeSkillset.ToList());
        }

        private void RemoveEmployeeSkills(Employee employee, Employee registeredEmployee)
        {
            var employeeSkillsToBeRemoved = registeredEmployee.EmployeeSkillset
                .Where(es => !employee.SkillIds.Contains(es.SkillId)).ToList();
            _employeeSkillRepository.RemoveEntries(employeeSkillsToBeRemoved);

            foreach (var employeeSkill in employeeSkillsToBeRemoved)
            {
                registeredEmployee.EmployeeSkillset.Remove(employeeSkill);
            }

            var skillsetIds = employeeSkillsToBeRemoved.Select(es => es.SkillId).ToList();
        }

        private Result ClearSkillsetAndSave(Employee registeredEmployee)
        {
            _employeeSkillRepository.RemoveEntries(registeredEmployee.EmployeeSkillset.ToList());
            var skillsetIds = registeredEmployee.EmployeeSkillset.Select(es => es.SkillId).ToList();

            foreach (var skill in registeredEmployee.EmployeeSkillset)
            {
                registeredEmployee.EmployeeSkillset.Remove(skill);
            }

            _employeeRepository.SaveEmployee(registeredEmployee);

            return _employeeRepository.SaveChanges();
        }

        private Result RegisterEmployee(Employee employee)
        {
            var result = new Result();

            if (employee.SkillIds != null && employee.SkillIds.Any())
            {
                RegisterSkillset(employee);
            }

            _employeeRepository.SaveEmployee(employee);
            var saveResult = _employeeRepository.SaveChanges();
            if (!saveResult.Success)
            {
                result.Message ="SaveEmployeeFailed";
                return result;
            }

            result.Success = true;
            return result;
        }

        private void RegisterSkillset(Employee employee)
        {
            foreach (var id in employee.SkillIds)
            {
                employee.EmployeeSkillset.Add(new EmployeeSkill
                {
                    Employee = employee,
                    SkillId = id
                });

                _employeeSkillRepository.AddEntries(employee.EmployeeSkillset.ToList());
            }
            
        }

        private void UpdateHistory(Employee employee, IEnumerable<int> skillIds, string verb)
        {
            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = verb,
                Target = employee
            };

            foreach (var id in skillIds)
            {
                historyEntry.ChangedSkills.Add(new SkillHistory
                {
                    HistoryEntry = historyEntry,
                    SkillId = id
                });
            }

            _historyRepository.LogEntry(historyEntry);
        }
    }

 
}
