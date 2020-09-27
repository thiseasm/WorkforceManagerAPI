namespace Domain.Models
{
    public class SkillHistory
    {
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public int HistoryEntryId { get; set; }
        
        public HistoryEntry HistoryEntry { get; set; }
    }
}
