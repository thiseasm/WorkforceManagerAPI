using System;
using System.Collections.Generic;

namespace WorkforceManagerAPI.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string HiredAt { get; set; }
        public ICollection<SkillViewModel> Skills { get; set; }
    }
}
