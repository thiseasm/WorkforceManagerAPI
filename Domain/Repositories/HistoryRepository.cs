using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly WorkforceContext _workforceDbContext;

        public HistoryRepository(WorkforceContext workforceContext)
        {
            _workforceDbContext = workforceContext ?? throw new ArgumentNullException();
        }

        public Result<List<HistoryEntry>> GetAll()
        {
            var result = new Result<List<HistoryEntry>>();
            try
            {
                result.Data = _workforceDbContext.History.OrderByDescending(h => h.CreatedAt).ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public Result<List<HistoryEntry>> GetEntriesForEmployee(int employeeId)
        {
            var result = new Result<List<HistoryEntry>>();
            try
            {
                result.Data = _workforceDbContext.History
                    .Where(h => h.Target.Id == employeeId)
                    .OrderByDescending(h => h.CreatedAt)
                    .ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public void LogEntry(HistoryEntry entry)
        {
            _workforceDbContext.History.Add(entry);
            _workforceDbContext.SaveChanges();
        }
    }
}
