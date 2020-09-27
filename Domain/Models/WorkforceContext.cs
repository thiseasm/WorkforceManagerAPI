using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class WorkforceContext  : DbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoryEntry> History { get; set; }
        
        public WorkforceContext(DbContextOptions<WorkforceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeSkill>().HasKey(es => new { es.EmployeeId, es.SkillId });
            modelBuilder.Entity<SkillHistory>().HasKey(sh => new { sh.SkillId, sh.HistoryEntryId });

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSkillset)
                .HasForeignKey(es => es.EmployeeId);


            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(es => es.Skill)
                .WithMany(s => s.EmployeeSkill)
                .HasForeignKey(es => es.SkillId);

            modelBuilder.Entity<SkillHistory>()
                .HasOne(sh => sh.Skill)
                .WithMany(s => s.SkillHistory)
                .HasForeignKey(sh => sh.SkillId);


            modelBuilder.Entity<SkillHistory>()
                .HasOne(sh => sh.HistoryEntry)
                .WithMany(he => he.ChangedSkills)
                .HasForeignKey(sh => sh.HistoryEntryId);
        }
    }
}