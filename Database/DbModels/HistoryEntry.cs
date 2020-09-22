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
        public Skill TargetSkill { get; set; }
        [Required]
        public string Description { get; set; }
    }
}