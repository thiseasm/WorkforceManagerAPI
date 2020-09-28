using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IHistoryRepository
    {
        GenericResult<List<HistoryEntry>> GetAll();
        GenericResult<List<HistoryEntry>> GetEntriesForEmployee(int employeeId);
        Result LogEntry(HistoryEntry entry);
        Result LogEntries(List<HistoryEntry> entries);
        Result SaveChanges();
    }
}
