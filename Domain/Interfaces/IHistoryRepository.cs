using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IHistoryRepository
    {
        Result<List<HistoryEntry>> GetAll();
        Result<List<HistoryEntry>> GetEntriesForEmployee(int employeeId);
        void LogEntry(HistoryEntry entry);
    }
}
