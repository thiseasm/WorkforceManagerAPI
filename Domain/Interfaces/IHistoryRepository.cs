using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IHistoryRepository
    {
        List<HistoryEntry> GetAll();
        List<HistoryEntry> GetEntriesForEmployee(int employeeId);
        void LogEntry(HistoryEntry entry);
    }
}
