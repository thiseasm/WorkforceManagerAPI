using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class WorkforceContext  : DbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoryEntry> History { get; set; }
        
        public WorkforceContext(DbContextOptions<WorkforceContext> options) : base(options) { }
    }
}