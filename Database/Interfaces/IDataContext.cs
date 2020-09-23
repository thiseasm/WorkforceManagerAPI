using Database.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Interfaces
{
    public interface IDataContext 
    {
        DbSet<Skill> Skills { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<HistoryEntry> History { get; set; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}