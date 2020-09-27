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

        public GenericResult<List<HistoryEntry>> GetAll()
        {
            var result = new GenericResult<List<HistoryEntry>>();
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

        public GenericResult<List<HistoryEntry>> GetEntriesForEmployee(int employeeId)
        {
            var result = new GenericResult<List<HistoryEntry>>();
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

        public Result LogEntry(HistoryEntry entry)
        {
            var result = new Result();
            try
            {
                _workforceDbContext.History.Add(entry);
                _workforceDbContext.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return  result
        }
    }
}
