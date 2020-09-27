using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Skill
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }

        public ICollection<EmployeeSkill> EmployeeSkill { get; set; }

        public ICollection<SkillHistory> SkillHistory { get; set; }
    }
}