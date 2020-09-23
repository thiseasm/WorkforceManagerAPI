using System.Collections.Generic;
using Database.DbModels;

namespace Repository.Interfaces
{
    public interface IHistoryRepository
    {
        List<HistoryEntry> GetAll();
        List<HistoryEntry> GetEntriesForEmployee(int employeeId);
    }
}
