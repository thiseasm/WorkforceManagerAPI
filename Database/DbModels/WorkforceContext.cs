using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Database.DbModels
{
    public class WorkforceContext  : DbContext, IDataContext
    {
        private IDbContextTransaction _transaction;

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoryEntry> History { get; set; }
        
        public WorkforceContext(DbContextOptions<WorkforceContext> options) : base(options) { }
        
        public void BeginTransaction()
        { 
            _transaction = Database.BeginTransaction();
        }
 
        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }        
        }
 
        public void Rollback()
        { 
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}