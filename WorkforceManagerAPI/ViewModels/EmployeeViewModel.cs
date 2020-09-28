using System.Collections.Generic;
using Domain.Models;

namespace WorkforceManagerAPI.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string HiredAt { get; set; }
        public List<SkillViewModel> Skills { get; set; }

        public EmployeeViewModel(Employee employee)
        {
            Id = employee.Id;
            Name = employee.Name;
            Surname = employee.Surname;
            HiredAt = employee.HiredAt.ToString("d");
            Skills = new List<SkillViewModel>();
        }
    }
}
