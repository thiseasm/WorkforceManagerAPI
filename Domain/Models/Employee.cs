using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Surname { get; set; }

        public DateTimeOffset HiredAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        
        public ICollection<Skill> Skillset { get; set; }
    }
}