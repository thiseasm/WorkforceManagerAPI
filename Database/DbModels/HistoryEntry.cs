using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.DbModels
{
    public class HistoryEntry
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Employee Target { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public ICollection<Skill> ChangedSkills { get; set; }
    }
}