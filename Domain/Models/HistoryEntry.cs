using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class HistoryEntry
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Employee Target { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        [Required]
        public ICollection<SkillHistory> ChangedSkills { get; set; } = new List<SkillHistory>();
    }
}