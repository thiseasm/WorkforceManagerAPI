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
        }

        private Result ClearSkillsetAndSave(Employee registeredEmployee)
        {
            _employeeSkillRepository.RemoveEntries(registeredEmployee.EmployeeSkillset.ToList());
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

        private static HistoryEntry GeneratedAddedEntry(Employee employee, IEnumerable<Skill> addedSkills)
        {
            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee
            };

            var changedSkills = addedSkills.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();
            historyEntry.ChangedSkills = changedSkills;
            return historyEntry;
        }

        private HistoryEntry GeneratedAddedEntry(Employee employee, Employee registeredEmployee)
        {
            var skillIdsAdded = employee.SkillIds
                                        .Where(id => !registeredEmployee.EmployeeSkillset
                                                .Select(s => s.SkillId)
                                                .Contains(id))
                                        .ToList();

            var getSkillsAddedResult = _skillRepository.GetSkillsById(skillIdsAdded);
            if (!getSkillsAddedResult.Success)
                return null;

            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Added",
                Target = employee,
            };
            var addedSkills = getSkillsAddedResult.Data.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();

            historyEntry.ChangedSkills = addedSkills;
            return historyEntry;
        }

        private HistoryEntry GenerateRemovedEntry(Employee employee, Employee registeredEmployee)
        {
            List<int> skillIdsRemoved;

            if (employee.SkillIds == null)
            {
                skillIdsRemoved = registeredEmployee.EmployeeSkillset.Select(s => s.SkillId).ToList();
            }
            else
            {
                skillIdsRemoved = registeredEmployee.EmployeeSkillset
                                    .Select(es => es.SkillId)
                                    .Where(e => !employee.SkillIds.Contains(e))
                                    .ToList();
            }
                
                

            var getSkillsRemovedResult = _skillRepository.GetSkillsById(skillIdsRemoved);
            if (!getSkillsRemovedResult.Success)
                return null;

            var historyEntry = new HistoryEntry
            {
                CreatedAt = DateTimeOffset.Now,
                Description = "Removed",
                Target = employee
            };

            var removedSkills = getSkillsRemovedResult.Data.Select(skill => new SkillHistory {HistoryEntry = historyEntry, Skill = skill}).ToList();

            historyEntry.ChangedSkills = removedSkills;
            return historyEntry;
        }
    }

 
}
