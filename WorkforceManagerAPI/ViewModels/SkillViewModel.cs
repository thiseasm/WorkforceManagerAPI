using Domain.Models;

namespace WorkforceManagerAPI.ViewModels
{
    public class SkillViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }

        public SkillViewModel(Skill skill)
        {
            Id = skill.Id;
            Title = skill.Title;
            Description = skill.Description;
            CreatedAt = skill.CreatedAt.ToString("d");
        }
    }
}
