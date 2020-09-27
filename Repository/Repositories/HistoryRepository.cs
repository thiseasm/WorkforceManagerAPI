using System;
using System.Collections.Generic;
using System.Linq;
using Database.DbModels;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public HistoryRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext ?? throw new ArgumentNullException();
        }

        public List<HistoryEntry> GetAll()
        {
            return _workforceDbContext.History.OrderByDescending(h => h.CreatedAt).ToList();
        }

        public List<HistoryEntry> GetEntriesForEmployee(int employeeId)
        {
            return _workforceDbContext.History
                .Where(h => h.Target.Id == employeeId)
                .OrderByDescending(h => h.CreatedAt)
                .ToList();
        }

        public void LogEntry(HistoryEntry entry)
        {
            _workforceDbContext.History.Add(entry);
            _workforceDbContext.SaveChanges();
        }
    }
}
