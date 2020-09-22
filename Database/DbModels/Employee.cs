using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.DbModels
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        
        public ICollection<Skill> Skillset { get; set; }
    }
}