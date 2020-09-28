using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IEmployeeSkillRepository
    {
        Result AddEntries(List<EmployeeSkill> employeeSkills);
        Result RemoveEntries(List<EmployeeSkill> employeeSkills);
    }
}
