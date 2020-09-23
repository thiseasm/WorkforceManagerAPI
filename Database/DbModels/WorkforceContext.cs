using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Database.DbModels
{
    public class WorkforceContext  : DbContext
    {
        private IDbContextTransaction _transaction;

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoryEntry> History { get; set; }
        
        public WorkforceContext(DbContextOptions<WorkforceContext> options) : base(options) { }
    }
}